using UnityEngine;
using UnityEditor;
using DG.DOTweenEditor;
using DG.Tweening;
using EGUIL = UnityEditor.EditorGUILayout;
using GUIL = UnityEngine.GUILayout;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;

[CustomEditor(typeof(MovableManager))]
public class MovableManagerEditor : Editor
{
    private int aIdx;
    private int bIdx;

    private Vector3 rawAPos;
    private Vector3 rawBPos;

    private MovableManager t;
    private GUIStyle labelStyle;

    private void Awake()
    {
    }
    private void OnEnable()
    {
        t = (MovableManager)target;
        bIdx = t.Movables.Count - 1;
        rawAPos = t.Movables[aIdx].position;
        rawBPos = t.Movables[bIdx].position;
        Debugger.SetLogPriority(LogBehaviour.Verbose);
    }

    private void OnDisable()
    {
        if (DOTweenEditorPreview.isPreviewing)
        {
            StopPreview();
        }
    }
    public override void OnInspectorGUI()
    {
        var needRedraw = false;
        var needRefreshPos = false;

        var preEnabled = GUI.enabled;
        GUI.enabled = !DOTweenEditorPreview.isPreviewing;
        DrawDefaultInspector();
        if (GUIL.Button("Collect hats"))
        {
            t.CollectHats();
            needRefreshPos = true;
        }
        if (GUIL.Button("Settleup hats"))
        {
            t.SettleupHats();
            needRefreshPos = true;
        }
        var max = t.Movables.Count - 1;

        EditorGUI.BeginChangeCheck();
        var newAIdx = EGUIL.IntSlider("Controll Point A", aIdx, 0, max);
        if (newAIdx != bIdx)
        {
            aIdx = newAIdx;
        }
        if (EditorGUI.EndChangeCheck() || needRefreshPos)
        {
            rawAPos = t.Movables[aIdx].position;
            needRedraw = true;
        }

        EditorGUI.BeginChangeCheck();
        var newBIdx = EGUIL.IntSlider("Controll Point B", bIdx, 0, max);
        if (newBIdx != aIdx)
        {
            bIdx = newBIdx;
        }
        if (EditorGUI.EndChangeCheck() || needRefreshPos)
        {
            rawBPos = t.Movables[bIdx].position;
            needRedraw = true;
        }

        GUI.enabled = preEnabled;
        var btnStr = DOTweenEditorPreview.isPreviewing ? "Stop Preview Path" : "Start Preview Path";
        if (GUIL.Button(btnStr))
        {
            if (!DOTweenEditorPreview.isPreviewing)
            {
                var (ta, tb) = t.CreateSwapAnimation(aIdx, bIdx);
                var tween = t.Combine(ta, tb);
                DOTweenEditorPreview.PrepareTweenForPreview(tween);
                DOTweenEditorPreview.Start();
            }
            else
            {
                StopPreview();
            }
        }
        if (needRedraw || needRefreshPos)
        {
            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }

    }

    void StopPreview()
    {
        DOTweenEditorPreview.Stop(true, true);
    }

    private void OnSceneGUI()
    {

        if (labelStyle == null)
        {
            labelStyle = new GUIStyle();
            labelStyle.normal.textColor = Color.red;
            labelStyle.fontSize = 100;
        }

        var worldAPos = t.APoint.Get(rawAPos, rawBPos);
        Handles.Label(worldAPos, "A", labelStyle);
        EditorGUI.BeginChangeCheck();
        var newWorldAPos = Handles.PositionHandle(worldAPos, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            t.APoint.Set(newWorldAPos, rawAPos, rawBPos);
        }

        var worldBPos = t.BPoint.Get(rawBPos, rawAPos);
        Handles.Label(worldBPos, "B", labelStyle);
        EditorGUI.BeginChangeCheck();
        var newWorldBPos = Handles.PositionHandle(worldBPos, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            t.BPoint.Set(newWorldBPos, rawBPos, rawAPos);
        }

        var preZTest = Handles.zTest;
        Handles.zTest = CompareFunction.LessEqual;
        Handles.DrawBezier(rawAPos, rawBPos, t.APoint.Get(rawAPos, rawBPos), t.BPoint.Get(rawBPos, rawAPos), Color.green, null, 2f);
        Handles.DrawBezier(rawBPos, rawAPos, t.APoint.Get(rawBPos, rawAPos), t.BPoint.Get(rawAPos, rawBPos), Color.red, null, 2f);
        Handles.zTest = preZTest;
    }
}
