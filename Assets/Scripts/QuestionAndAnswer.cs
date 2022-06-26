using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

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

public class QuestionAndAnswer : MonoBehaviour
{

    public Text text;
    public TextAsset Json;
    public GameObject buttonPrefab;
    public CanvasGroup cg;
    private QuestionItem questionItem;
    private AnswerButton[] buttons;
    public ScriptableRendererFeature feature;

    // Start is called before the first frame update
    void Start()
    {
        EventsCenter.AskQuestion += OnAsk;
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
            feature.SetActive(true);
            // ѡС��
            if (EventsCenter.ChooseHat != null)
                EventsCenter.ChooseHat.Invoke(this, new EventArgs());
        }
        else if(questionItem.type == 1)
        {
            feature.SetActive(false);
            buttons = new AnswerButton[questionItem.options.Length];
            int half = questionItem.options.Length / 2;
            // ѡ��
            for (int i = 0; i < questionItem.options.Length; i++)
            {
                buttons[i] = Instantiate(buttonPrefab, cg.gameObject.transform).GetComponent<AnswerButton>();
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
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    public void DisabledUI()
    {
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        cg.interactable = false;
        feature.SetActive(false);
    }

    public void OnMakeAnswer(object sender, EventArgs answerIndex)
    {
        DisabledUI();
        if(EventsCenter.EndDialogue != null)
            EventsCenter.EndDialogue(this, new EventArgs());
    }
}
