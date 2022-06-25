using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RoundManager : MonoBehaviour
{
    public List<Round> round = new List<Round>();
    public Action<int> endFunc;

    public int nowIdx = 0;
    // Use this for initialization

    private void Start()
    {
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
        Debug.Log("���ݳ�ʼ�����");
        startGame();
    }

    public void startGame()
    {
        // ��ʼ��Ϸ
        Debug.Log("��ʼ��Ϸ");
        MovableManager move = gameObject.GetComponent<MovableManager>();
        move.init(endFunc);
    }

    public void selectGame(int idx)
    {
        Debug.Log("ѡ��𰸽׶�,��Ϊ"+idx);
        Dialogue talk = gameObject.GetComponent<Dialogue>();
        talk.init();
        // ѡ��׶�
        if (round[nowIdx].endType == "1")
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