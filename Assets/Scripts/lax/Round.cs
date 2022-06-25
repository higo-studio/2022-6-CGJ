using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Round
{
    // 球的颜色ID
    [SerializeField]
    public int ballColor;

    // 帽子的颜色ID
    [SerializeField]
    public int hatColor;

    // 帽子的转速
    [SerializeField]
    public float hatSpeed;

    // 帽子转的次数
    [SerializeField]
    public int changeNum;

    // 帽子的个数
    [SerializeField]
    public int hatNum;

    // 结局类型：选小球还是输入,1为选小球，2为输入
    [SerializeField]
    public int endType;
}
