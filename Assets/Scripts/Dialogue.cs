using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class JsonArrayWrap<T>
{
    public T[] items;
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


    void start()
    {
        DisabledUI();
        Debug.Log("设置不可见");
        // test
    }

    public void init() {
        Speak(Json);
    }

    void SetJson(TextAsset jsonAseet)
    {
        items = JsonUtility.FromJson<JsonArrayWrap<DialogueItem>>(jsonAseet.text).items;
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

    public void Speak(TextAsset jsonAsset)
    {
        speaking = true;
        SetJson(jsonAsset);
        EnableUI();
        Next(0);
    }

    public void Finished()
    {
        speaking = false;
        DisabledUI();
        // callback
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