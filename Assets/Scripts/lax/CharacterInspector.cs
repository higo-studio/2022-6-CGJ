using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(RoundManager))]
public class CharacterInspector : Editor
{
    ReorderableList reorderableList;

    void OnEnable()
    {
        SerializedProperty prop = serializedObject.FindProperty("round");

        reorderableList = new ReorderableList(serializedObject, prop, true, true, true, true);

        //设置单个元素的高度
        reorderableList.elementHeight = 120;

        //绘制单个元素
        reorderableList.drawElementCallback =
            (rect, index, isActive, isFocused) => {
                var element = prop.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);
            };

        //背景色
        reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {
            GUI.backgroundColor = Color.black;
        };

        //头部
        reorderableList.drawHeaderCallback = (rect) =>
            EditorGUI.LabelField(rect, prop.displayName);

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}