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
        // 初始化当前关卡的设置
        Debug.Log("数据初始化完成");
        startGame();
    }

    public void startGame()
    {
        // 开始游戏
        Debug.Log("开始游戏");
        MovableManager move = gameObject.GetComponent<MovableManager>();
        move.init(endFunc);
    }

    public void selectGame(int idx)
    {
        Debug.Log("选择答案阶段,答案为"+idx);
        Dialogue talk = gameObject.GetComponent<Dialogue>();
        talk.init();
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