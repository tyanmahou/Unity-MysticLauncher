using UnityEditor;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// Shell編集
    /// </summary>
    public class ShellScriptEditWindow : EditorWindow
    {
        public static void Show(SerializedProperty property)
        {
            ShellScriptEditWindow window = CreateInstance<ShellScriptEditWindow>();
            window.titleContent = new GUIContent("Shell Script Edit");
            window.Init(property);
            window.ShowUtility();
        }
        public void Init(SerializedProperty property)
        {
            _property = property;
            _windows = property.FindPropertyRelative(nameof(PlatformShellScript.Windows));
            _osx = property.FindPropertyRelative(nameof(PlatformShellScript.OSX));
            _autoPause = property.FindPropertyRelative(nameof(PlatformShellScript.AutoPause));
            _workingDir = property.FindPropertyRelative(nameof(PlatformShellScript.WorkingDirectory));

            _selectedTab = DefaultTab();
        }

        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
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
            EditorGUIUtil.DrawSeparator();
            {
                EditorGUILayout.PropertyField(_autoPause);
                EditorGUILayout.PropertyField(_workingDir);

                using (new EditorGUI.DisabledGroupScope(_selectedTab != DefaultTab()))
                {
                    var runContent = EditorGUIUtil.NewIconContent("Play", "Run", "Execute Scripts");
                    if (GUILayout.Button(runContent))
                    {
                        var script = new PlatformShellScript()
                        {
                            Windows = _windows.stringValue,
                            OSX = _osx.stringValue,
                            AutoPause = _autoPause.boolValue,
                            WorkingDirectory = _workingDir.stringValue,
                        };
                        TerminalUtil.Exec(script);
                    }
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                _property.serializedObject.ApplyModifiedProperties();
            }
        }
        void EditWindows()
        {
            _windows.stringValue = EditorGUILayout.TextArea(_windows.stringValue, GUILayout.ExpandHeight(true));
        }
        void EditOSX()
        {
            _osx.stringValue = EditorGUILayout.TextArea(_osx.stringValue, GUILayout.ExpandHeight(true));
        }
        int DefaultTab()
        {
#if UNITY_EDITOR_WIN
            return 0;
#else
            return 1;
#endif
        }
        private void OnDestroy()
        {
        }
        private SerializedProperty _property;
        private SerializedProperty _windows;
        private SerializedProperty _osx;
 
        private SerializedProperty _autoPause;
        private SerializedProperty _workingDir;
        private Vector2 _scrollPosition;
        private int _selectedTab = 0;
    }
}
