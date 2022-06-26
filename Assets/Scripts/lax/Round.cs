using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Round
{
    // ���̺��
    // �����ɫID
    [SerializeField]
    [Range(0, 3)]
    public int ballColor;

    // ñ�ӵ���ɫID
    [SerializeField]
    public GameObject HatPrefab;

    // ñ��ת�Ĵ���
    [SerializeField]
    [Range(0, 50)]
    public int changeNum;

    // ñ�ӵĸ���
    [SerializeField]
    [Range(3, 10)]
    public int hatNum;

    [SerializeField]
    [Range(1, 5)]
    public int parallelNum = 1;

    // ������ͣ�ѡС��������,1ΪѡС��2Ϊ����
    [SerializeField]
    public RoundEndType endType;

    [SerializeField]
    public AnimationCurve SpeedCurve;

    [SerializeField]
    public TextAsset Json;
}

public enum RoundEndType
{
    SelectBall, TextInput
}