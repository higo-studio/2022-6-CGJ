using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DG.Tweening;

public class RoundManager : MonoBehaviour
{
    public List<Round> round = new List<Round>();

    public Action<int> endFunc;

    private int nowIdx = 0;
    MovableManager move;

    private void Start()
    {
        move = gameObject.GetComponent<MovableManager>();
        endFunc += selectGame;
        this.initRound(nowIdx);
    }

    public void nextRound()
    {
        nowIdx++;
        this.initRound(nowIdx);
    }

    public void initRound(int idx)
    {
        // 初始化当前关卡的设置
        move.Rounds = round[nowIdx].changeNum;
        move.HatCount = round[nowIdx].hatNum;
        startGame();
    }

    public void startGame()
    {
        // 开始游戏
        Debug.Log("开始游戏");
        move.init(endFunc);
    }

    private TaskCompletionSource<int> hatRayCastSource;
    public async void selectGame(int idx)
    {
        Dialogue talk = gameObject.GetComponent<Dialogue>();
        talk.init();
        // 选择阶段
        if (round[nowIdx].endType == RoundEndType.SelectBall)
        {
            // 选择小球
            hatRayCastSource = new TaskCompletionSource<int>();
            var hitIdx = await hatRayCastSource.Task;

            var isCorret = hitIdx == idx;
            move.CreatePutBallAnimation(move.GetEndIndexFromStartIndex(hitIdx), isCorret).AppendCallback(() =>
            {
                if (isCorret)
                {
                    Debug.Log("选择成功");
                }
                else
                {
                    Debug.Log("选择失败");
                }
            });
        }
        else
        {
            // 输入文字
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
                Debug.Log(hitInfo.transform.name);
                hatRayCastSource.SetResult(hatComp.Index);
                hatRayCastSource = null;
            }
        }
    }

    public void endGame()
    {
        // 结束阶段
        if(round.Count> nowIdx+1)
        {
            // 下一关
            nextRound();
        }
        else
        {
            Debug.Log("恭喜你通关啦");
        }
    }
}