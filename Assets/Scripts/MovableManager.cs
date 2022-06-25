using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyBox;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using DG.Tweening.Plugins;

[Serializable]
public class RelativePoint
{
    [SerializeField]
    private Vector3 value;

    public void Set(Vector3 point, Vector3 start, Vector3 end)
    {
        var ray = end - start;
        var dir = ray.normalized;
        var root = Matrix4x4.TRS(start, Quaternion.LookRotation(dir), Vector3.one);
        value = root.inverse.MultiplyPoint(point);
    }

    public Vector3 Get(Vector3 start, Vector3 end)
    {
        var ray = end - start;
        var dir = ray.normalized;
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

    private Queue<(int, int)> swapTuples;
    // Start is called before the first frame update
    void Start()
    {
        CollectHats();
        SettleupHats();

        swapTuples = new Queue<(int, int)>();
        Append((0, 1), (1, 6), (2, 6), (3, 5));
        Next();
    }

    void Append(params (int, int)[] tuples)
    {
        foreach (var tuple in tuples)
        {
            swapTuples.Enqueue(tuple);
        }
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
        var center = (Movables.Count - 1) / 2f;
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
        return (aT, bT);
    }

    public Tween Combine(Tween ta, Tween tb)
    {
        var seq = DOTween.Sequence();
        seq.Join(ta).Join(tb);
        return seq;
    }

    public void Next()
    {
        if (!swapTuples.TryDequeue(out var tuple))
        {
            Debug.Log("Finish");
        }
        else
        {
            Swap(tuple.Item1, tuple.Item2).AppendCallback(() => Next());
        }
    }

    public Sequence Swap(int aIdx, int bIdx)
    {
        var (ta, tb) = CreateSwapAnimation(aIdx, bIdx);
        var t = Combine(ta, tb);
        return DOTween.Sequence().Append(t).AppendCallback(() =>
        {
            var temp = Movables[aIdx];
            Movables[aIdx] = Movables[bIdx];
            Movables[bIdx] = temp;
        });
    }
}