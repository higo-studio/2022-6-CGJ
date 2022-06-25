using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIShake : MonoBehaviour
{
    public float strange = 20;
    void Start()
    {
        transform.GetComponent<RectTransform>().DOShakePosition(0.5f, strange, 50, 180).SetLoops(-1);
    }
}
