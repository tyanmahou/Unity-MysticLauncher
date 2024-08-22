using UnityEditor;
using UnityEngine;

namespace Mystic
{

    [CustomPropertyDrawer(typeof(FileSelectAttribute))]
    public class FileSelectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            FileSelectAttribute fileSelectAttribute = attribute as FileSelectAttribute;

            bool enabledOld = GUI.enabled;
            // ラベルを描画
            EditorGUI.LabelField(position, label);

            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth + 30;
            GUI.enabled = enabledOld && fileSelectAttribute.FreeInput;
            EditorGUI.TextField(position, property.stringValue);
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
                    property.stringValue = path;
                }
            }
            GUI.enabled = enabledOld;
        }

        GUIContent _icon;
    }

}