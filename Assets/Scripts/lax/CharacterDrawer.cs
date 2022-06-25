using UnityEngine;
using System.Collections;
using UnityEditor;

//定制Serializable类的每个实例的GUI
[CustomPropertyDrawer(typeof(Round))]
public class CharacterDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //创建一个属性包装器，用于将常规GUI控件与SerializedProperty一起使用
        using (new EditorGUI.PropertyScope(position, label, property))
        {
            //设置属性名宽度 Name HP
            EditorGUIUtility.labelWidth = 60;
            //输入框高度，默认一行的高度
            position.height = EditorGUIUtility.singleLineHeight;


            Rect ballRect = new Rect(position)
            {
                //在name的基础上，y坐标下移
                y = EditorGUIUtility.singleLineHeight + 10
            };

            Rect hatRect = new Rect(ballRect)
            {
                //在hp的基础上，y坐标下移
                y = ballRect.y + EditorGUIUtility.singleLineHeight + 2
            };

            Rect speedRect = new Rect(hatRect)
            {
                //在hp的基础上，y坐标下移
                y = hatRect.y + EditorGUIUtility.singleLineHeight + 2
            };

            Rect changeRect = new Rect(speedRect)
            {
                //在hp的基础上，y坐标下移
                y = speedRect.y + EditorGUIUtility.singleLineHeight + 2
            };

            Rect hatNumRect = new Rect(changeRect)
            {
                //在hp的基础上，y坐标下移
                y = changeRect.y + EditorGUIUtility.singleLineHeight + 2
            };

            Rect endRect = new Rect(hatNumRect)
            {
                width = position.width,
                y = hatNumRect.y + EditorGUIUtility.singleLineHeight + 2
            };

            //找到每个属性的序列化值
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