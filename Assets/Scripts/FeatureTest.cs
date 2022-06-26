using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.Universal;

public class FeatureTest : MonoBehaviour
{
    public ScriptableRendererFeature feature;

    public void FeatureSwitch()
    {
        feature.SetActive(!feature.isActive);
    }
           
}

#if UNITY_EDITOR
[CustomEditor(typeof(FeatureTest))]
public class FeatureTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var tar = target as FeatureTest;
        base.OnInspectorGUI();
        if (GUILayout.Button("Feature Switch Test"))
        {
            tar.FeatureSwitch();
        }
    }
}
#endif