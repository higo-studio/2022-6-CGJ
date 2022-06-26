using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Round
{
    // 蓝绿红白
    // 球的颜色ID
    [SerializeField]
    [Range(0, 3)]
    public int ballColor;

    // 帽子的颜色ID
    [SerializeField]
    public GameObject HatPrefab;

    // 帽子转的次数
    [SerializeField]
    [Range(0, 50)]
    public int changeNum;

    // 帽子的个数
    [SerializeField]
    [Range(3, 10)]
    public int hatNum;

    [SerializeField]
    [Range(1, 5)]
    public int parallelNum = 1;

    // 结局类型：选小球还是输入,1为选小球，2为输入
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