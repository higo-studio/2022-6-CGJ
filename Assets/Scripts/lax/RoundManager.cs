using UnityEngine;
using System.Collections.Generic;
using System;

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

    public void selectGame(int idx)
    {
        Dialogue talk = gameObject.GetComponent<Dialogue>();
        talk.init();
        // ѡ��׶�
        if (round[nowIdx].endType == 1)
        {
            // ѡ��С��
        }
        else
        {
            // ��������
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