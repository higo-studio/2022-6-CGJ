using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class QuestionWrap
{
    public QuestionItem question;
}

[Serializable]
public class QuestionItem
{
    public int type;
    public string question;
    public string[] options;
}

[RequireComponent(typeof(CanvasGroup))]
public class QuestionAndAnswer : MonoBehaviour
{

    public Text text;
    public TextAsset Json;
    public GameObject buttonPrefab;
    private QuestionItem questionItem;
    private AnswerButton[] buttons;
    private CanvasGroup cg;

    // Start is called before the first frame update
    void Start()
    {
        EventsCenter.AskQuestion += OnAsk;
        cg = GetComponent<CanvasGroup>();
        EventsCenter.MakeAnswer += OnMakeAnswer;
        DisabledUI();
    }

    public void Init(TextAsset jsonAsset)
    {
        Json = jsonAsset;
        questionItem = JsonUtility.FromJson<QuestionWrap>(jsonAsset.text).question;
        ShowQuestion();
        if (questionItem.type == 0)
        {
            // 选小球
            if (EventsCenter.ChooseHat != null)
                EventsCenter.ChooseHat.Invoke(this, new EventArgs());
        }
        else if(questionItem.type == 1)
        {
            Debug.Log(questionItem.options.Length);
            buttons = new AnswerButton[questionItem.options.Length];
            int half = questionItem.options.Length / 2;
            // 选项
            for (int i = 0; i < questionItem.options.Length; i++)
            {
                buttons[i] = Instantiate(buttonPrefab).GetComponent<AnswerButton>();
                buttons[i].transform.SetParent(transform);
                buttons[i].SetPositionByOffset(i - half);
                buttons[i].Init(i, questionItem.options[i]);
            }
        }
        EnableUI();
    }

    public void ShowQuestion()
    {
        text.text = questionItem.question;
    }

    public void OnAsk(object sender, EventArgs e)
    {
        Init(Json);
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

    public void OnMakeAnswer(object sender, EventArgs answerIndex)
    {
        DisabledUI();
    }
}
