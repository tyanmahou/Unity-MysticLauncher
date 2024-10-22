using System;
using System.Linq;
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

            // ヘッダー
            DrawHeader();
            EditorGUIUtil.DrawSeparator();

            // 上のビュー
            using (_splitter.SplitTop())
            {
                // リスト描画
                DrawMemoList();
            }
            // 下のビュー
            using(_splitter.SplitBottom())
            {
                // 編集ビュー
                using (EditorGUIUtil.ScopedMargin())
                {
                    if (UserNotePad.instance.IsLockEdit)
                    {
                        DrawMemo();
                    }
                    else
                    {
                        DrawEdit();
                    }
                }
            }
        }
        private void DrawHeader()
        {
            GUILayout.Space(5);
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                string prevSearch = _searchString;
                _searchString = _searchField.OnGUI(_searchString);
                if (EditorGUIUtil.IconButton("d_Toolbar Plus@2x", "New Memo"))
                {
                    CreateNew();
                }
                using (new EditorGUI.DisabledScope(UserNotePad.instance.SelectedMemo is null))
                    if (EditorGUIUtil.IconButton("d_Toolbar Minus@2x", "Remove Memo"))
                    {
                        RemoveMemo(UserNotePad.instance.SelectedMemo);
                    }
                if (EditorGUIUtil.IconButton(
                    UserNotePad.instance.IsLockEdit ? "Unlocked@2x" : "Locked@2x",
                    UserNotePad.instance.IsLockEdit ? "Lock Edit" : "Unlock Edit"
                    ))
                {
                    UserNotePad.instance.IsLockEdit = !UserNotePad.instance.IsLockEdit;
                    UserNotePad.instance.Save();
                    _serializedObject.Update();
                }
            }
            GUILayout.Space(5);
        }
        private void DrawMemoList()
        {
            var notePad = UserNotePad.instance;
            bool Filter(MemoEntry memo)
            {
                return memo.Title.IsSearched(_searchString) || memo.Text.IsSearched(_searchString);
            }
            var memos = notePad
                .EnumerateMemos
                .Select((memo, index) => (memo, index)) // index付与
                .Where(memoIndex => Filter(memoIndex.memo)) // サーチ
                .Reverse();
            int viewIndex = 0;
            foreach (var (memo, index) in memos)
            {
                DrawEntry(memo, index, viewIndex);
                ++viewIndex;
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
        private void DrawEntry(MemoEntry entry, int index, int viewIndex)
        {
            var dateStyle = new GUIStyle(EditorStyles.label);
            dateStyle.fontSize = 11;
            var subTextStyle = new GUIStyle(EditorStyles.label);
            subTextStyle.fontSize = 11;
            subTextStyle.richText = true;

            var notePad = UserNotePad.instance;

            var rect = GUILayoutUtility.GetRect(0, 54);
            var bg = EditorGUIUtil.ListBackGroundColor(viewIndex);
            if (index == notePad.SelectIndex)
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

                        menu.AddItem(new GUIContent("Remove"), false, () => RemoveMemo(entry));
                        menu.ShowAsContext();
                    }
                }
                float width = rect.width;
                float height = rect.height;
                rect.x += 4;
                rect.y += 2;
                // 日付
                rect.height = 18;
                if (!string.IsNullOrEmpty(entry.URL))
                {
                    rect.width = width - 22 - 4;
                }
                EditorGUI.LabelField(rect, entry.CreatedAt, dateStyle);
                rect.y += 18;
                rect.width = 32;
                rect.height = 32;
                if (entry.Icon.TryGetGUIContent(out var icon))
                {
                    var iconRect = rect;
                    iconRect.x += 2;
                    iconRect.y += 2;
                    iconRect.width -= 4;
                    iconRect.height -= 4;
                    GUI.DrawTexture(iconRect, icon.image, ScaleMode.ScaleToFit);
                }
                rect.x += 32;
                rect.width = width - 32 - 4;
                rect.height = 18;
                EditorGUIUtil.TruncateFit(rect, entry.Title, EditorStyles.boldLabel);
                rect.y += 18;
                rect.height = 14;
                EditorGUIUtil.TruncateFit(rect, entry.Text.Split('\n')[0], subTextStyle);
            }
        }
        private void DrawMemo()
        {
            var notePad = UserNotePad.instance;
            var entry = notePad.SelectedMemo;
            if (entry is null)
            {
                if (GUILayout.Button("New"))
                {
                    CreateNew();
                }
                return;
            }
            using (new GUILayout.HorizontalScope())
            {
                var iconRect = EditorGUIUtil.GetFixRect(32, 32);
                if (entry.Icon.TryGetGUIContent(out var icon))
                {
                    iconRect.x += 2;
                    iconRect.y += 2;
                    iconRect.width -= 4;
                    iconRect.height -= 4;
                    GUI.DrawTexture(iconRect, icon.image, ScaleMode.ScaleToFit);
                }
                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Space(8);
                    EditorGUILayout.LabelField(entry.Title, EditorStyles.boldLabel, GUILayout.MinWidth(0));
                }
            }
            EditorGUIUtil.DrawSeparator();
            {
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.wordWrap = true;
                labelStyle.richText = true;
                labelStyle.alignment = TextAnchor.UpperLeft;
                EditorGUILayout.SelectableLabel(entry.Text, labelStyle, GUILayout.ExpandHeight(true));
            }
            foreach (var asset in entry.Assets)
            {
                var content = EditorGUIUtility.ObjectContent(asset, typeof(UnityEngine.Object));
                if (GUILayout.Button(content, EditorStyles.objectField, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    EditorGUIUtility.PingObject(asset);
                    if (_doubleClick.DoubleClick())
                    {
                        AssetDatabase.OpenAsset(asset);
                    }
                }
            }
            if (!string.IsNullOrEmpty(entry.URL))
            {
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(EditorGUIUtil.NewIconContent("d_Linked", tooltip: "link URL"), GUILayout.Width(16));
                    if (EditorGUILayout.LinkButton(entry.URL, GUILayout.MinWidth(0)))
                    {
                        Application.OpenURL(entry.URL);
                    }
                }
            }
            EditorGUIUtil.DrawSeparator();
            {
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Created", GUILayout.Width(50));
                    EditorGUILayout.LabelField(":  " + entry.CreatedAt, GUILayout.MinWidth(0));
                }
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Updated", GUILayout.Width(50));
                    EditorGUILayout.LabelField(":  " + entry.UpdatedAt, GUILayout.MinWidth(0));
                }
            }
        }

        private void DrawEdit()
        {
            var notePad = UserNotePad.instance;

            _serializedObject.Update();
            int selectIndex = _serializedObject.FindProperty("SelectIndex").intValue;
            var memo = notePad.Memo(selectIndex);
            if (memo is null)
            {
                if (GUILayout.Button("New"))
                {
                    CreateNew();
                }
                return;
            }
            var memosProp = _serializedObject.FindProperty("_memos");
            var memoProp = memosProp.GetArrayElementAtIndex(selectIndex);
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
                notePad.Save();
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
        MemoEntry CreateNew()
        {
            var notePad = UserNotePad.instance;
            var memo = new MemoEntry();
            Undo.RecordObject(notePad, "Create Memo");
            notePad.Register(memo);
            notePad.SelectIndex = notePad.Count - 1;
            notePad.IsLockEdit = false;
            notePad.Save();
            return memo;
        }
        void RemoveMemo(MemoEntry entry)
        {
            var notePad = UserNotePad.instance;
            Undo.RecordObject(notePad, "Remove Memo");
            notePad.Unregister(entry);
            if (notePad.SelectIndex >= notePad.Count)
            {
                notePad.SelectIndex = notePad.Count - 1;
            }
            notePad.Save();
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
        DoubleClickCtrl _doubleClick = new();

        SerializedObject _serializedObject;

        SearchField _searchField = new();
        string _searchString = string.Empty;
    }
}
