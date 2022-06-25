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
        // ��ʼ����ǰ�ؿ�������
        move.Rounds = round[nowIdx].changeNum;
        move.HatCount = round[nowIdx].hatNum;
        startGame();
    }

    public void startGame()
    {
        // ��ʼ��Ϸ
        Debug.Log("��ʼ��Ϸ");
        move.init(endFunc);
    }

    private TaskCompletionSource<int> hatRayCastSource;
    public async void selectGame(int idx)
    {
        Dialogue talk = gameObject.GetComponent<Dialogue>();
        talk.init();
        // ѡ��׶�
        if (round[nowIdx].endType == RoundEndType.SelectBall)
        {
            // ѡ��С��
            hatRayCastSource = new TaskCompletionSource<int>();
            var hitIdx = await hatRayCastSource.Task;

            var isCorret = hitIdx == idx;
            move.CreatePutBallAnimation(move.GetEndIndexFromStartIndex(hitIdx), isCorret).AppendCallback(() =>
            {
                if (isCorret)
                {
                    Debug.Log("ѡ��ɹ�");
                }
                else
                {
                    Debug.Log("ѡ��ʧ��");
                }
            });
        }
        else
        {
            // ��������
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
        // �����׶�
        if(round.Count> nowIdx+1)
        {
            // ��һ��
            nextRound();
        }
        else
        {
            Debug.Log("��ϲ��ͨ����");
        }
    }
}