using UnityEditor;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// Shell編集
    /// </summary>
    public class ShellEditWindow : EditorWindow
    {
        public static void Show(SerializedProperty property)
        {
            ShellEditWindow window = CreateInstance<ShellEditWindow>();
            window.titleContent = new GUIContent("Shell Edit");
            window.Init(property);
            window.ShowAuxWindow();
        }
        public void Init(SerializedProperty property)
        {
            _property = property;
            _windows = property.FindPropertyRelative("Windows");
            _osx = property.FindPropertyRelative("OSX");

#if UNITY_EDITOR_WIN
            _selectedTab = 0;
#else
            _selectedTab = 1;
#endif
        }

        void OnGUI()
        {
            int nextTab = GUILayout.Toolbar(
                _selectedTab,
                new string[] { "Windows", "OSX" },
                EditorStyles.toolbarButton,
                GUI.ToolbarButtonSize.FitToContents
                );
            if (nextTab != _selectedTab )
            {
                _selectedTab = nextTab;
                GUIUtility.keyboardControl = 0;
            }
            EditorGUIUtil.DrawSeparator();
            // スクロールビュー開始
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            switch(_selectedTab)
            {
                case 0:
                    EditWindows();
                    break;
                case 1:
                    EditOSX();
                    break;
            }
            EditorGUILayout.EndScrollView();
        }
        void EditWindows()
        {
            EditorGUI.BeginChangeCheck();
            _windows.stringValue = EditorGUILayout.TextArea(_windows.stringValue, GUILayout.ExpandHeight(true));
            if (EditorGUI.EndChangeCheck())
            {
                _windows.serializedObject.ApplyModifiedProperties();
            }
        }
        void EditOSX()
        {
            EditorGUI.BeginChangeCheck();
            _osx.stringValue = EditorGUILayout.TextArea(_osx.stringValue, GUILayout.ExpandHeight(true));
            if (EditorGUI.EndChangeCheck())
            {
                _osx.serializedObject.ApplyModifiedProperties();
            }
        }
        private void OnDestroy()
        {
        }
        private SerializedProperty _property;
        private SerializedProperty _windows;
        private SerializedProperty _osx;
        private Vector2 _scrollPosition;
        private int _selectedTab = 0;
    }
}
