using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RoundManager : MonoBehaviour
{
    public List<Round> round = new List<Round>();
    public int nowIdx = 0;
    // Use this for initialization
    void Start()
    {

    }

    public void nextRound()
    {
        nowIdx++;
        this.initRound(nowIdx);
    }

    public void initRound(int idx)
    {
        // ��ʼ����ǰ�ؿ�������

        startGame();
    }

    public void startGame()
    {
        // ��ʼ��Ϸ
        selectGame();
    }

    public void selectGame()
    {
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