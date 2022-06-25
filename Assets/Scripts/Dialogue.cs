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
    public TMPro.TMP_Text Speaker;
    public TextAsset Json;
    public float ShowWordsDuration = 2;

    private DialogueItem[] items;

    private bool speaking = false;
    private float timer = 0;
    private int currIndex;

    private void Awake()
    {

    }

    void Start()
    {
        DisabledUI();
        // test
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
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    public void DisabledUI()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

    public void ShowWords(string str)
    {
        Speaker.SetText(str);
    }
}