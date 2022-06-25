using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsCenter
{
    public static EventHandler AskQuestion;
    public static EventHandler MakeAnswer;
    public static EventHandler ChooseHat;
}

public class AnswerArg : EventArgs
{
    public int answer;
    public AnswerArg(int answer)
    {
        this.answer = answer;
    }
}
