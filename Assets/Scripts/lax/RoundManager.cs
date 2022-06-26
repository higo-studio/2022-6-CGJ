using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DG.Tweening;

public class RoundManager : MonoBehaviour
{
    public List<Round> round = new List<Round>();

    public Action<int> endFunc;

    public GameObject selectUI;

    private int nowIdx = 1;
    MovableManager move;
    Dialogue talk;
    QuestionAndAnswer qa;

    public ScriptableRendererFeature feature;
    public GraphicRaycaster raycaster;
    public LayerMask everything;
    public LayerMask nothing;


    private void Start()
    {
        selectUI.SetActive(false);
        move = gameObject.GetComponent<MovableManager>();
        talk = gameObject.GetComponent<Dialogue>();
        qa = gameObject.GetComponent<QuestionAndAnswer>();
        endFunc += selectGame;
        this.initRound(nowIdx);
    }

    public void nextRound()
    {
        selectUI.SetActive(false);
        nowIdx++;
        this.initRound(nowIdx);
    }

    public void initRound(int idx)
    {
        // 初始化当前关卡的设置
        move.Rounds = round[nowIdx].changeNum;
        move.HatCount = round[nowIdx].hatNum;
        move.SpeedCurve = round[nowIdx].SpeedCurve;
        move.HatPrefab = round[nowIdx].HatPrefab;
        talk.Json = round[nowIdx].Json;
        qa.Json = round[nowIdx].Json;
        startGame();
    }

    public void startGame()
    {
        selectUI.SetActive(false);
        // 开始游戏
        Debug.Log("开始游戏");
        move.init(endFunc);
    }

    private TaskCompletionSource<int> hatRayCastSource;
    public async void selectGame(int idx)
    {
        talk.EnableUI();
        selectUI.SetActive(true);
        // 选择阶段
        if (round[nowIdx].endType == RoundEndType.SelectBall)
        {
            Dialogue talk = gameObject.GetComponent<Dialogue>();
            talk.Speaker.text = "那么，小球在哪顶帽子里面呢？";
            // 选择小球

            feature.SetActive(true);
            raycaster.blockingMask = nothing;

            hatRayCastSource = new TaskCompletionSource<int>();
            var hitIdx = await hatRayCastSource.Task;

            var isCorret = hitIdx == idx;
            move.CreatePutBallAnimation(move.GetEndIndexFromStartIndex(hitIdx), isCorret).AppendCallback(() =>
            {
                int result = isCorret ? 1 : 0;
                if (EventsCenter.EndDialogue != null)
                    EventsCenter.EndDialogue(this, new WrongOrRight(result));
                feature.SetActive(false);
                raycaster.blockingMask = everything;

            });
            Debug.Log("下一关");
        }
        else
        {
            // 输入文字
            if (EventsCenter.AskQuestion != null) { }
            EventsCenter.AskQuestion.Invoke(this, new EventArgs());
        }
    }

    private void Update()
    {
        if (hatRayCastSource != null && Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, 1 << 6) 
                && hitInfo.transform.TryGetComponent<Hat>(out var hatComp))
            {
                hatRayCastSource.SetResult(hatComp.Index);
                hatRayCastSource = null;
            }
        }
    }

    public void endGame(bool success)
    {

        if (!success)
        {
            Debug.Log("失败了，重来");
            startGame();
            return;
        }

        // 结束阶段
        if(round.Count> nowIdx+1)
        {
            Debug.Log("本关结束，下一关~");
            // 下一关
            nextRound();
        }
        else
        {
            Debug.Log("恭喜你通关啦");
        }
    }
}