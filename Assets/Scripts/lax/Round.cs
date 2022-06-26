using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Round
{
    // �����ɫID
    [SerializeField]
    [Range(0, 5)]
    public int ballColor;

    // ñ�ӵ���ɫID
    [SerializeField]
    [Range(0, 5)]
    public int hatColor;

    // ñ��ת�Ĵ���
    [SerializeField]
    [Range(0, 50)]
    public int changeNum;

    // ñ�ӵĸ���
    [SerializeField]
    [Range(3, 10)]
    public int hatNum;

    // ������ͣ�ѡС��������,1ΪѡС��2Ϊ����
    [SerializeField]
    public RoundEndType endType;

    [SerializeField]
    public AnimationCurve SpeedCurve;
}

public enum RoundEndType
{
    SelectBall, TextInput
}