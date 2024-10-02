using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class FontSpriteConverter : EditorWindow
    {
        TMPro.TMP_SpriteAsset _asset;

        [MenuItem("Window/FontSpriteConverter")]
        public static void ShowWindow()
        {
            GetWindow<FontSpriteConverter>("FontSpriteConverter");
        }
        void OnGUI()
        {
            _asset = EditorGUILayout.ObjectField(_asset, typeof(TMPro.TMP_SpriteAsset), false) as TMPro.TMP_SpriteAsset;

            if (GUILayout.Button("Save"))
            {
                string path = EditorUtility.SaveFilePanel("Save Emoji Data", "Assets", "NewEmojiData.asset", "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    path = FileUtil.GetProjectRelativePath(path);
                    // アセットを保存
                    var newAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
                    Copy(_asset, newAsset);
                    AssetDatabase.CreateAsset(newAsset, path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }
        private static void Copy(TMP_SpriteAsset original, TMP_SpriteAsset copy)
        {
            // TMP_SpriteAssetのすべてのフィールドを取得
            FieldInfo[] fields = typeof(TMP_SpriteAsset).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                // 元のフィールドの値を取得
                object value = field.GetValue(original);
                // 新しいインスタンスのフィールドに値を設定
                field.SetValue(copy, value);
            }
        }
    }
}
