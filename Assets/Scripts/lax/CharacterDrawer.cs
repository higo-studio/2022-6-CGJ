using UnityEngine;
using System.Collections;
using UnityEditor;

//����Serializable���ÿ��ʵ����GUI
[CustomPropertyDrawer(typeof(Round))]
public class CharacterDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //����һ�����԰�װ�������ڽ�����GUI�ؼ���SerializedPropertyһ��ʹ��
        using (new EditorGUI.PropertyScope(position, label, property))
        {
            //������������� Name HP
            EditorGUIUtility.labelWidth = 60;
            //�����߶ȣ�Ĭ��һ�еĸ߶�
            position.height = EditorGUIUtility.singleLineHeight;


            Rect ballRect = new Rect(position)
            {
                //��name�Ļ����ϣ�y��������
                y = EditorGUIUtility.singleLineHeight + 10
            };

            Rect hatRect = new Rect(ballRect)
            {
                //��hp�Ļ����ϣ�y��������
                y = ballRect.y + EditorGUIUtility.singleLineHeight + 2
            };

            Rect speedRect = new Rect(hatRect)
            {
                //��hp�Ļ����ϣ�y��������
                y = hatRect.y + EditorGUIUtility.singleLineHeight + 2
            };

            Rect changeRect = new Rect(speedRect)
            {
                //��hp�Ļ����ϣ�y��������
                y = speedRect.y + EditorGUIUtility.singleLineHeight + 2
            };

            Rect hatNumRect = new Rect(changeRect)
            {
                //��hp�Ļ����ϣ�y��������
                y = changeRect.y + EditorGUIUtility.singleLineHeight + 2
            };

            Rect endRect = new Rect(hatNumRect)
            {
                width = position.width,
                y = hatNumRect.y + EditorGUIUtility.singleLineHeight + 2
            };

            //�ҵ�ÿ�����Ե����л�ֵ
            SerializedProperty ballColor = property.FindPropertyRelative("ballColor");
            SerializedProperty hatColor = property.FindPropertyRelative("hatColor");
            SerializedProperty hatSpeed = property.FindPropertyRelative("hatSpeed");
            SerializedProperty changeNum = property.FindPropertyRelative("changeNum");
            SerializedProperty hatNum = property.FindPropertyRelative("hatNum");
            SerializedProperty endType = property.FindPropertyRelative("endType");


            EditorGUI.IntSlider(ballRect, ballColor, 0, 5);

            EditorGUI.IntSlider(hatRect, hatColor, 0, 5);

            EditorGUI.IntSlider(speedRect, hatSpeed, 0, 100);

            EditorGUI.IntSlider(changeRect, changeNum, 0, 50);

            EditorGUI.IntSlider(hatNumRect, hatNum, 3, 10);

            endType.stringValue = EditorGUI.TextField(endRect, endType.displayName, endType.stringValue);
        }
    }
}