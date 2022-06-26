using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening; 

public class UIEpisode : MonoBehaviour
{
    public int CurrentEp = 1;
    public int EpNum = 15;
    public GameObject EpPrefab;
    public RoundManager roundManager;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < roundManager.round.Count-1; i++)//第0关有bug所以-1
        {
            Instantiate(EpPrefab, transform);
        }
        transform.GetChild(CurrentEp - 1).GetComponent<RectTransform>().sizeDelta = new Vector2(50, 20);
        transform.GetChild(CurrentEp - 1).GetComponent<RectTransform>().localRotation = Quaternion.identity;
    }

    public void NextEp()
    {
        if (CurrentEp >= EpNum)
        {
            return;
        }
        transform.GetChild(CurrentEp - 1).GetComponent<RectTransform>().DOSizeDelta(new Vector2(20, 20), 0.2f);
        transform.GetChild(CurrentEp - 1).GetComponent<RectTransform>().DOLocalRotate(new Vector3(0, 0, 45), 0.2f);
        CurrentEp++;
        transform.GetChild(CurrentEp - 1).GetComponent<RectTransform>().DOSizeDelta(new Vector2(50, 20), 0.2f);
        transform.GetChild(CurrentEp - 1).GetComponent<RectTransform>().DOLocalRotate(Vector3.zero, 0.2f);
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(UIEpisode))]
public class UIEpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var tar = target as UIEpisode;
        base.OnInspectorGUI();
        if (GUILayout.Button("Next Ep Test"))
        {
            tar.NextEp();
        }
    }
}
#endif

