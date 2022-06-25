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

        //���õ���Ԫ�صĸ߶�
        reorderableList.elementHeight = 120;

        //���Ƶ���Ԫ��
        reorderableList.drawElementCallback =
            (rect, index, isActive, isFocused) => {
                var element = prop.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);
            };

        //����ɫ
        reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {
            GUI.backgroundColor = Color.black;
        };

        //ͷ��
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