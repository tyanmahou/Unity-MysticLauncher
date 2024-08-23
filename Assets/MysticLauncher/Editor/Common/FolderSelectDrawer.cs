using UnityEditor;
using UnityEngine;

namespace Mystic
{

    [CustomPropertyDrawer(typeof(FolderSelectAttribute))]
    public class FolderSelectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            FolderSelectAttribute folderSelectAttribute = attribute as FolderSelectAttribute;

            bool enabledOld = GUI.enabled;
            // ラベルを描画
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.width -= 30;
            GUI.enabled = enabledOld && folderSelectAttribute.FreeInput;
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
                string path = EditorUtility.OpenFolderPanel(
                    folderSelectAttribute.Title,
                    folderSelectAttribute.Folder,
                    folderSelectAttribute.DefaultName
                    );
                if (!string.IsNullOrEmpty(path))
                {
                    property.stringValue = path;
                }
            }
            GUI.enabled = enabledOld;
            EditorGUI.EndProperty();
        }

        GUIContent _icon;
    }

}
