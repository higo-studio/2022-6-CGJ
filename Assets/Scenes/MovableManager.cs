using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyBox;
using DG.Tweening;
using DG.Tweening.Core;

[Serializable]
public class RelativePoint
{
    [SerializeField]
    private Vector3 value;

    public void Set(Vector3 point, Vector3 start, Vector3 end)
    {
        var dir = (end - start).normalized;
        var root = Matrix4x4.TRS(start, Quaternion.LookRotation(dir), Vector3.one);
        value = root.inverse.MultiplyPoint(point);
    }

    public Vector3 Get(Vector3 start, Vector3 end)
    {
        var dir = (end - start).normalized;
        var root = Matrix4x4.TRS(start, Quaternion.LookRotation(dir), Vector3.one);
        return root.MultiplyPoint(value);
    }
}

public class MovableManager : MonoBehaviour
{
    [Range(0.5f, 5)]
    public float Duration = 2f;
    public Transform Container;
    public RelativePoint APoint;
    public RelativePoint BPoint;
    [Header("Ô¤ÀÀÇø")]
    [ReadOnly]
    public List<Transform> Movables;
    // Start is called before the first frame update
    void Start()
    {
        CollectHats();
    }

    public void CollectHats()
    {
        Movables.Clear();
        foreach (Transform trs in Container)
        {
            Movables.Add(trs);
        }
    }

    public void SettleupHats()
    {
        var center = Movables.Count / 2f;
        for (var i = 0; i < Movables.Count; i++)
        {
            Movables[i].position = new Vector3((i - center) * 4, 0, 0);
        }
    }

    public (Tween a, Tween b) CreateSwapAnimation(int aIdx, int bIdx)
    {
        var a = Movables[aIdx];
        var b = Movables[bIdx];
        var aT = a.DOPath(
            new Vector3[] { b.transform.position, APoint.Get(a.position, b.position), BPoint.Get(b.position, a.position) }
            , Duration, PathType.CubicBezier, PathMode.Sidescroller2D);
        var bT = b.DOPath(
            new Vector3[] { a.transform.position, APoint.Get(b.position, a.position), BPoint.Get(a.position, b.position) }
            , Duration, PathType.CubicBezier, PathMode.Sidescroller2D);

        //aT.PathGetDrawPoints
        return (aT, bT);
    }

    public Tween Combine(Tween ta, Tween tb)
    {
        var seq = DOTween.Sequence();
        seq.Join(ta).Join(tb);
        return seq;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
