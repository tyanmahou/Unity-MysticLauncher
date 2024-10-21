using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Mystic
{
    public class NotePadLayout : ITabLayout
    {
        public string Title => "NotePad";
        public Icon Icon => Icon.CreateUnityIcon("d_TextScriptImporter Icon");

        public void OnGUI()
        {
            LauncherWindow.Instance.Repaint();
            if (_serializedObject is null)
            {
                _serializedObject = new SerializedObject(UserNotePad.instance);
            }
            var notePad = UserNotePad.instance;

            using (_splitter.SplitTop())
            {
                if (_dateStyle == null)
                {
                    _dateStyle = new GUIStyle(EditorStyles.label);
                    _dateStyle.fontSize = 11;
                }
                int index = 0;
                foreach (var memo in notePad.EnumerateMemos)
                {
                    DrawEntry(memo, notePad.Count - 1 -index);
                    ++index;
                }
                var rest = GUILayoutUtility.GetRect(0, 0, GUILayout.MinHeight(50), GUILayout.ExpandHeight(true));
                if (GUI.Button(rest, GUIContent.none, GUIStyle.none))
                {
                    _serializedObject.FindProperty("SelectIndex").intValue = -1;
                    _serializedObject.ApplyModifiedProperties();
                    GUIUtility.keyboardControl = 0;
                }
                GUILayoutUtility.GetRect(0, 0, GUILayout.Height(4));
            }
            using(_splitter.SplitBottom())
            {
                using (EditorGUIUtil.ScopedMargin())
                {
                    DrawEdit();
                }
            }
        }
        private void DrawEntry(MemoEntry entry, int index)
        {
            var rect = GUILayoutUtility.GetRect(0, 54);
            var bg = EditorGUIUtil.ListBackGroundColor(UserNotePad.instance.Count - 1 - index);
            if (index == UserNotePad.instance.SelectIndex)
            {
                bg = EditorGUIUtil.ListSelectedBackGroundColor();
            }
            else if (rect.Contains(Event.current.mousePosition))
            {
                bg = EditorGUIUtil.ListHoverBackGroundColor();
            }
            EditorGUI.DrawRect(rect, bg);
            {
                // URL
                if (!string.IsNullOrEmpty(entry.URL))
                {
                    var iconRect = rect;
                    iconRect.x = iconRect.width - 18 - 4;
                    iconRect.y += 4;
                    iconRect.height = 18;
                    iconRect.width = 18;
                    var content = EditorGUIUtil.NewIconContent("d_Linked", tooltip: $"Open Url {entry.URL}");
                    if (GUI.Button(iconRect, content, EditorStyles.iconButton))
                    {
                        Application.OpenURL(entry.URL);
                    }
                }
                if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
                {
                    if (Event.current.button == 0)
                    {
                        _serializedObject.FindProperty("SelectIndex").intValue = index;
                        _serializedObject.ApplyModifiedProperties();
                        GUIUtility.keyboardControl = 0;
                    }
                    else if (Event.current.button == 1)
                    {
                        GenericMenu menu = new GenericMenu();

                        menu.AddItem(new GUIContent("Remove"), false, () =>
                        {
                            Undo.RecordObject(UserNotePad.instance, "Remove Memo");
                            UserNotePad.instance.SelectIndex = -1;
                            UserNotePad.instance.Unregister(entry);
                            UserNotePad.instance.Save();
                        });
                        menu.ShowAsContext();
                    }
                }
                float width = rect.width;
                float height = rect.height;
                rect.y += 2;
                // 日付
                rect.height = 18;
                if (!string.IsNullOrEmpty(entry.URL))
                {
                    rect.width = width - 22;
                }
                EditorGUI.LabelField(rect, entry.CreatedAt, _dateStyle);
                rect.y += 18;
                rect.width = 32;
                rect.height = 32;
                if (entry.Icon.TryGetGUIContent(out var icon))
                {
                    GUI.DrawTexture(rect, icon.image, ScaleMode.ScaleToFit);
                }
                rect.x += 32;
                rect.width = width - 32;
                rect.height = 18;
                EditorGUIUtil.TruncateFit(rect, entry.Title, EditorStyles.boldLabel);
                rect.y += 18;
                rect.height = 14;
                EditorGUIUtil.TruncateFit(rect, entry.Text.Split('\n')[0], _dateStyle);
            }
        }
        private void DrawEdit()
        {
            _serializedObject.Update();
            var memo = UserNotePad.instance.Memo(_serializedObject.FindProperty("SelectIndex").intValue);
            if (memo is null)
            {
                if (GUILayout.Button("New"))
                {
                    memo = new MemoEntry();
                    Undo.RecordObject(UserNotePad.instance, "Create Memo");
                    UserNotePad.instance.Register(memo);
                    UserNotePad.instance.Save();
                }
                return;
            }
            var memosProp = _serializedObject.FindProperty("_memos");
            var memoProp = memosProp.GetArrayElementAtIndex(UserNotePad.instance.SelectIndex);
            if (_reorderableEntries is null)
            {
                _reorderableEntries = new ReorderableList(null, typeof(UnityEngine.Object), true, true, true, true)
                {
                    drawElementCallback = DrawEntryElement,
                    drawHeaderCallback = DrawEntryHeader,
                };
            }
            EditorGUI.BeginChangeCheck();
            using (new GUILayout.HorizontalScope())
            {
                var iconRect = EditorGUIUtil.GetFixRect(32, 32);
                if (GUI.Button(iconRect, memo.Icon.GetGUIContent()))
                {
                    var iconProp = memoProp.FindPropertyRelative("Icon");
                    IconPickerWindow.ShowFromIcon(iconProp);
                }
                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Space(8);
                    var titleProp = memoProp.FindPropertyRelative("Title");
                    titleProp.stringValue = EditorGUIUtil.TextPlaceholderFeild(titleProp.stringValue, "Title", GUILayout.MinWidth(0));
                }
            }
            var textProp = memoProp.FindPropertyRelative("Text");
            var textArea = new GUIStyle(EditorStyles.textField);
            textArea.wordWrap = true;
            textProp.stringValue = EditorGUILayout.TextArea(textProp.stringValue, textArea, GUILayout.ExpandHeight(true), GUILayout.MinWidth(0));

            var assetsProp = memoProp.FindPropertyRelative("Assets");
            _reorderableEntries.serializedProperty = assetsProp;
            _reorderableEntries.DoLayoutList();
            using (new GUILayout.HorizontalScope())
            {
                var urlProp = memoProp.FindPropertyRelative("URL");
                EditorGUILayout.LabelField(EditorGUIUtil.NewIconContent("d_Linked", tooltip: "link URL"), GUILayout.Width(16));
                urlProp.stringValue = EditorGUIUtil.TextPlaceholderFeild(urlProp.stringValue, "Link URL", GUILayout.MinWidth(0));
            }
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Created", GUILayout.Width(50));
                EditorGUILayout.LabelField(":  " + memo.CreatedAt, GUILayout.MinWidth(0));
            }
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Updated", GUILayout.Width(50));
                EditorGUILayout.LabelField(":  " + memo.UpdatedAt, GUILayout.MinWidth(0));
            }
            if (EditorGUI.EndChangeCheck())
            {
                memo.UpdateOpenedAt();
                _serializedObject.ApplyModifiedProperties();
                UserNotePad.instance.Save();
            }
        }
        private void DrawEntryHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Asset");
        }
        private void DrawEntryElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _reorderableEntries.serializedProperty.GetArrayElementAtIndex(index);
            element.objectReferenceValue = EditorGUI.ObjectField(rect, element.objectReferenceValue, typeof(UnityEngine.Object), false);
        }
        public override string ToString()
        {
            return Title;
        }
        VerticalSplitter _splitter = new(
            separatorInit: h => h - 240,
            separatorMin: h => h / 2,
            separatorMax: h => Mathf.Max(h / 2, h - 40)
            );
        ReorderableList _reorderableEntries;

        SerializedObject _serializedObject;
        static GUIStyle _dateStyle;
    }
}
