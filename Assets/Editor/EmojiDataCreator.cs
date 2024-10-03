using System.Reflection;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class EmojiDataCreator : EditorWindow
    {
        TextAsset _emojiJson;

        [MenuItem("Tools/EmojiDataCreator")]
        public static void ShowWindow()
        {
            GetWindow<EmojiDataCreator>("EmojiDataCreator");
        }
        void OnGUI()
        {
            _emojiJson = EditorGUILayout.ObjectField(_emojiJson, typeof(TextAsset), false) as TextAsset;

            using (new EditorGUI.DisabledScope(_emojiJson is null))
            {
                if (GUILayout.Button("Save"))
                {
                    string path = EditorUtility.SaveFilePanel("Save Emoji Data Asset", "Assets/MysticLauncher/Resources", "EmojiData.asset", "asset");
                    if (!string.IsNullOrEmpty(path))
                    {
                        path = FileUtil.GetProjectRelativePath(path);
                        // アセットを保存
                        try
                        {
                         
                            var newAsset = ScriptableObject.CreateInstance<EmojiDataList>();
                            string json = "{\"_emojis\":" + _emojiJson.text + "}";
                            JsonUtility.FromJsonOverwrite(json, newAsset);
                            AssetDatabase.CreateAsset(newAsset, path);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                        catch(System.Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    }
                }
            }
        }
    }
}
