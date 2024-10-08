﻿using UnityEditor;
using UnityEngine;

namespace Mystic
{

    [CustomPropertyDrawer(typeof(FileSelectAttribute))]
    public class FileSelectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int i = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(position, label, property);
            FileSelectAttribute fileSelectAttribute = attribute as FileSelectAttribute;

            bool enabledOld = GUI.enabled;
            // ラベルを描画
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;

            position.width -= 30;
            GUI.enabled = enabledOld && fileSelectAttribute.FreeInput;
            property.stringValue = EditorGUI.TextField(position, property.stringValue);
            if (_icon == null)
            {
                _icon = EditorGUIUtility.IconContent("d_Folder Icon");
            }
            GUI.enabled = enabledOld && true;
            position.x += position.width;
            position.width = 30;
            if (GUI.Button(position, _icon))
            {
                // ファイル選択ダイアログを表示
                string path = EditorUtility.OpenFilePanel(
                    fileSelectAttribute.Title,
                    fileSelectAttribute.Directory,
                    fileSelectAttribute.Extension
                    );
                if (!string.IsNullOrEmpty(path))
                {
                    path = PathUtil.RelativePathInProject(path);
                    property.stringValue = path;
                }
            }
            GUI.enabled = enabledOld;
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = i;
        }

        GUIContent _icon;
    }

}
