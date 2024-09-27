using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Mystic
{
    [CustomEditor(typeof(UserFavorite))]
    public class UserFavoriteEditor : Editor
    {
        public void OnEnable()
        {
            _entries = serializedObject.FindProperty("Entries");

            _reorderableEntries = new ReorderableList(_entries.serializedObject, _entries, true, true, true, true)
            {
                drawElementCallback = DrawEntryElement,
                drawHeaderCallback = DrawEntryHeader,
            };
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();
            {
                var rect = EditorGUILayout.GetControlRect();
                _reorderableEntries.DoList(rect);
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }

        private void DrawEntryHeader(Rect rect)
        {
            const float offset = 18;
            rect.x += offset;
            rect.width -= offset;
            var groupPos = rect;
            groupPos.width = rect.width / 2 - 4 -26;
            EditorGUI.LabelField(groupPos, "Group");

            var assetPos = groupPos;
            assetPos.x += rect.width / 2 - 26;
            EditorGUI.LabelField(assetPos, "Asset");
        }
        private void DrawEntryElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            const float margin = 4;
            rect.height -= margin;
            rect.y += margin / 2;
            var element = _reorderableEntries.serializedProperty.GetArrayElementAtIndex(index);
            var group = element.FindPropertyRelative("FavoriteGroup");
            var asset = element.FindPropertyRelative("Asset");

            var groupPos = rect;
            groupPos.width = rect.width / 2 - 4 -26;
            group.stringValue = EditorGUI.TextField(groupPos, group.stringValue);

            var assetPos = rect;
            assetPos.width = rect.width / 2 - 4;
            assetPos.x += rect.width / 2 - 26;
            asset.objectReferenceValue = EditorGUI.ObjectField(assetPos, asset.objectReferenceValue, typeof(UnityEngine.Object), false);

            var favPos = assetPos;
            favPos.x += assetPos.width + 4;
            favPos.width = 26;
            if (GUI.Button(favPos, EditorGUIUtil.NewIconContent("d_Favorite_colored", tooltip: "Show Edit Favorite Window")))
            {
                FavoriteWindow.Show(asset.objectReferenceValue, group.stringValue);
            }
        }
        SerializedProperty _entries;
        private ReorderableList _reorderableEntries;
    }
}
