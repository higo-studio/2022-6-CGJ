using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsCenter
{
    public static EventHandler AskQuestion;         // ���죬��ʼ������
    public static EventHandler MakeAnswer;          // ѡ��һ��ѡ�ѡ��ģʽ��
    public static EventHandler ChooseHat;           // ��AskQuestionͬʱ��������ѡñ��ģʽ��
    public static EventHandler StarterDialogue;
    public static EventHandler EndDialogue;
    public static EventHandler StartMove;
}

// ���ڴ���ѡ��Ĵ�
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

