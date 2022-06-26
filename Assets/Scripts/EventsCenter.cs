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
    public static EventHandler StartMove;
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

public class WrongOrRight : EventArgs
{
    public int answer;     // 0 = f    1 = t
    public WrongOrRight(int answer)
    {
        this.answer = answer;
    }
}

