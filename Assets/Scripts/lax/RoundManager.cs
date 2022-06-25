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
        // 初始化当前关卡的设置

        startGame();
    }

    public void startGame()
    {
        // 开始游戏
        selectGame();
    }

    public void selectGame()
    {
        // 选择阶段
        if (round[nowIdx].endType == "1")
        {
            // 选择小球
        }
        else
        {
            // 输入文字
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