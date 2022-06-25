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
    static System.Random Random = new System.Random();
    [Range(0.5f, 5)]
    public float Duration = 2f;
    public Transform Container;
    public RelativePoint APoint;
    public RelativePoint BPoint;
    [Header("Ô¤ÀÀÇø")]
    [ReadOnly]
    public List<Transform> Movables;
    public Transform Ball;

    private Queue<(int, int)> swapTuples;
    // Start is called before the first frame update
    void Start()
    {
        swapTuples = new Queue<(int, int)>();
        CollectHats();
        SettleupHats();

        var ballIdx = Random.Next(0, Movables.Count);
        for (var i = 0; i < 10; i++)
        {
            Append(GetTwoRandomInt(Movables.Count));
        }
        CreatePutBallAnimation(ballIdx).AppendCallback(() => Next());
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

    public Sequence CreatePutBallAnimation(int idx)
    {
        var movable = Movables[idx];
        var seq = DOTween.Sequence();

        var upSide = Vector3.up * 3;
        var rotSide = new Vector3(45, 0, 0);

        var ballPos = movable.position;
        ballPos.y = 0.5f;
        Ball.position = ballPos;
        Ball.gameObject.SetActive(true);
        seq.Append(movable.DOBlendableLocalMoveBy(upSide, 1f));
        seq.Join(movable.DOBlendableLocalRotateBy(rotSide, 1f));
        seq.AppendInterval(2f);
        seq.Append(movable.DOBlendableLocalMoveBy(-upSide, 1f));
        seq.Join(movable.DOBlendableLocalRotateBy(-rotSide, 1f));
        seq.AppendCallback(() => Ball.gameObject.SetActive(false));
        return seq;
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

    static (int, int) GetTwoRandomInt(int count)
    {
        var arr = new int[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = i;
        }
        ShuffleCopy(arr);
        return (arr[0], arr[count - 1]);
    }

    static T[] ShuffleCopy<T>(T[] arr)
    {

        for (var i = arr.Length - 1; i > 0; --i)
        {
            int randomIndex = Random.Next(i + 1);

            T temp = arr[i];
            arr[i] = arr[randomIndex];
            arr[randomIndex] = temp;
        }

        return arr;
    }
}