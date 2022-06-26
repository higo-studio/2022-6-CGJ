using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class StartJsonArrayWrap<T>
{
    public T[] startItems;
}

[Serializable]
public class EndJsonArrayWrap<T>
{
    public T[] endItems;
}
[Serializable]
public class DialogueItem
{
    public string speaker;
    public string content;
    public string menuText;
    public int[] linkTo;
}

public class Dialogue : MonoBehaviour
{
    public Text Speaker;
    public TextAsset Json;
    public float ShowWordsDuration = 2;

    private DialogueItem[] items;

    private bool speaking = false;
    private float timer = 0;
    private int currIndex;

    private void Awake()
    {
        EventsCenter.StarterDialogue += OnStarterDialouge;
        EventsCenter.EndDialogue += OnEnderDialouge;
    }

    void Start()
    {
        DisabledUI();
        Debug.Log("设置不可见");
        // test
    }

    public void OnStarterDialouge(object sender, EventArgs args)
    {
        Speak(Json, 0);
    }

    public void OnEnderDialouge(object sender, EventArgs args)
    {
        Speak(Json, 1);
    }

    // type : 0 = 开始      1 = 问题后的对话
    void SetJson(TextAsset jsonAseet, int type)
    {
        if(type == 0)
        {
            items = JsonUtility.FromJson<StartJsonArrayWrap<DialogueItem>>(Json.text).startItems;
        }
        else if(type == 1)
        {
            items = JsonUtility.FromJson<EndJsonArrayWrap<DialogueItem>>(Json.text).endItems;
        }
    }

    void Update()
    {
        if (!speaking)
            return;
        timer += Time.deltaTime;
        if (timer > ShowWordsDuration)
        {
            if (items[currIndex].linkTo.Length <= 0)
                Next(currIndex + 1);
            else
                Next(items[currIndex].linkTo[0]);
            timer = 0;
        }
    }

    public void Next(int index)
    {
        if (index < 0)
        {
            Finished();
            return;
        }
        currIndex = index;
        ShowWords(items[currIndex].content);
    }

    public void Speak(TextAsset jsonAsset, int type)
    {
        speaking = true;
        SetJson(jsonAsset, type);
        EnableUI();
        Next(0);
    }

    public void Finished()
    {
        speaking = false;
        DisabledUI();
    }

    public void EnableUI()
    {
        Speaker.gameObject.SetActive(true);
    }

    public void DisabledUI()
    {
        Speaker.gameObject.SetActive(false);
    }

    public void ShowWords(string str)
    {
        Speaker.text = str;
    }
}