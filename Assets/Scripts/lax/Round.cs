using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Round
{
    // �����ɫID
    [SerializeField]
    public int ballColor;

    // ñ�ӵ���ɫID
    [SerializeField]
    public int hatColor;

    // ñ�ӵ�ת��
    [SerializeField]
    public float hatSpeed;

    // ñ��ת�Ĵ���
    [SerializeField]
    public int changeNum;

    // ñ�ӵĸ���
    [SerializeField]
    public int hatNum;

    // ������ͣ�ѡС��������,1ΪѡС��2Ϊ����
    [SerializeField]
    public int endType;
}
