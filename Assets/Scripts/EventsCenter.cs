using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsCenter
{
    public static EventHandler AskQuestion;         // 闭嘴，开始问问题
    public static EventHandler MakeAnswer;          // 选了一个选项（选项模式）
    public static EventHandler ChooseHat;           // 和AskQuestion同时被触发（选帽子模式）
    public static EventHandler StarterDialogue;
    public static EventHandler EndDialogue;
}

// 用于传递选择的答案
public class AnswerArg : EventArgs
{
    public int answer;
    public AnswerArg(int answer)
    {
        this.answer = answer;
    }
}

