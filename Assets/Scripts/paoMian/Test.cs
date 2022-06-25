using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventsCenter.MakeAnswer += OnAnswer;
    }

    public void OnAnswer(object sender, EventArgs answerIndex)
    {

        Debug.Log($"—°‘Ò¡À{((AnswerArg)answerIndex).answer}∫≈");
    }
}
