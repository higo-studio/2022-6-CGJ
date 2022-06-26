using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class AnswerButton : MonoBehaviour
{

    public Button button;
    public float interval = 200;
    public int Index;
    public string Option;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Click);
    }

    public void Init(int index, string option)
    {
        Index = index;
        Option = option;
        button.GetComponentInChildren<Text>().text = option;
    }

    public void SetPositionByOffset(int index)
    {
        //GetComponent<RectTransform>().localPosition = new Vector3(index * interval, 0, 0);
    }

    public void Click()
    {
        Debug.Log($"???????????????  {Index}  ??????????????");
        if(EventsCenter.MakeAnswer != null)
            EventsCenter.MakeAnswer.Invoke(this, new AnswerArg(Index));
    }
}
