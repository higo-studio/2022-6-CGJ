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

