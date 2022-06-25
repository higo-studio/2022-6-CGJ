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

    public Vector3 Get(Vector3 start, Vector3 end, float scalar = 1f)
    {
        var ray = end - start;
        var dir = ray.normalized;
        var root = Matrix4x4.TRS(start, Quaternion.LookRotation(dir), Vector3.one);
        return root.MultiplyPoint(value * scalar);
    }
}

public class MovableManager : MonoBehaviour
{
    static System.Random Random = new System.Random();
    [Range(0.2f, 5)]
    public float Duration = 2f;
    public Transform Container;
    public RelativePoint APoint;
    public RelativePoint BPoint;
    public Transform Ball;
    public AnimationCurve SpeedCurve;
    public GameObject HatPrefab;
    public int HatCount = 7;
    public int ParallelCount = 2;
    public int Rounds = 10;
    [Header("Ô¤ÀÀÇø")]
    [ReadOnly]
    public List<Transform> Movables;

    private Queue<(int, int)> swapTuples;
    private int currentRound = -1;
    private int[] indexs;
    private int startIdx;
    // Start is called before the first frame update
    void Start()
    {
        swapTuples = new Queue<(int, int)>();

        CollectHats();
        SettleupHats();
        indexs = new int[Movables.Count];
        indexs.FillBy(i => i);
        startIdx = Random.Next(0, Movables.Count);
        for (var i = 0; i < Rounds; i++)
        {
            //Append(GetTwoRandomInt(Movables.Count));
            Append(GetRandomInts(Movables.Count, ParallelCount));
        }
        CreatePutBallAnimation(startIdx).AppendCallback(() => Next());
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
        if (HatCount == Movables.Count) return;
        Movables.Clear();
        foreach (Transform trs in Container)
        {
            Destroy(trs.gameObject);
        }
        Container.DetachChildren();
        for (var i = 0; i < HatCount; i++)
        {
            Movables.Add(Instantiate(HatPrefab, Container).transform);
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

    public (Tween a, Tween b) CreateSwapAnimation(int aIdx, int bIdx, float scalar = 1f)
    {
        var a = Movables[aIdx];
        var b = Movables[bIdx];
        var aT = a.DOPath(
            new Vector3[] { b.transform.position, APoint.Get(a.position, b.position, scalar), BPoint.Get(b.position, a.position, scalar) }
            , Duration, PathType.CubicBezier, PathMode.Sidescroller2D);
        var bT = b.DOPath(
            new Vector3[] { a.transform.position, APoint.Get(b.position, a.position, scalar), BPoint.Get(a.position, b.position, scalar) }
            , Duration, PathType.CubicBezier, PathMode.Sidescroller2D);
        return (aT, bT);
    }

    public Sequence CreatePutBallAnimation(int idx, int delay = 0)
    {
        var movable = Movables[idx];
        var seq = DOTween.Sequence();

        var upSide = Vector3.up * 3;
        var rotSide = new Vector3(45, 0, 0);

        var ballPos = movable.position;
        ballPos.y = 0.5f;
        Ball.position = ballPos;
        Ball.gameObject.SetActive(true);
        if (delay > 0)
        {
            seq.AppendInterval(delay);
        }
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
        var factor = SpeedCurve.Evaluate((float)++currentRound / Rounds);
        Duration = Mathf.Lerp(0.2f, 1.5f, factor);
        if (swapTuples.Count < ParallelCount)
        {
            CreatePutBallAnimation(indexs.FirstIndex(val => val == startIdx), 4).AppendCallback(() => Debug.Log("Finish"));
            return;
        }
        
        var seq = DOTween.Sequence();
        for (int i = 0; i < ParallelCount; i++)
        {
            var tuple = swapTuples.Dequeue();
            seq.Join(Swap(tuple.Item1, tuple.Item2, 1 + (ParallelCount - i - 1) * 0.5f));
        }
        seq.AppendCallback(() => Next());
    }

    public Sequence Swap(int aIdx, int bIdx, float animationScalr = 1f)
    {
        var (ta, tb) = CreateSwapAnimation(aIdx, bIdx, animationScalr);
        var t = Combine(ta, tb);
        return DOTween.Sequence().Append(t).AppendCallback(() =>
        {
            var temp = Movables[aIdx];
            Movables[aIdx] = Movables[bIdx];
            Movables[bIdx] = temp;
            
            var temp1 = indexs[aIdx];
            indexs[aIdx] = indexs[bIdx];
            indexs[bIdx] = temp1;
        });
    }

    static (int, int)[] GetRandomInts(int count, int parallelCount)
    {
        if (count < parallelCount * 2) throw new Exception("!!!!!!!!!!");
        var arr = new int[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = i;
        }
        ShuffleCopy(arr);
        var subArr = arr[..(parallelCount * 2)];
        Array.Sort(subArr);

        var outs = new (int, int)[parallelCount];
        for (var i = 0; i < parallelCount; i++)
        {
            outs[i] = (subArr[i], subArr[subArr.Length - 1 - i]);
        }
        return outs;
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