using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mystic
{
    [Serializable]
    public class AssetElement : IElement
    {
        public UnityEngine.Object Asset;

        public void OnGUI()
        {
            using var horizontal = new EditorGUILayout.HorizontalScope();
            var content = EditorGUIUtility.ObjectContent(Asset, typeof(UnityEngine.Object));
            if (GUILayout.Button(content, EditorStyles.objectField, GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                EditorGUIUtility.PingObject(Asset);
            }
            var openContent = new GUIContent()
            {
                image = AssetPreview.GetMiniThumbnail(Asset)
            };
            if (GUILayout.Button(openContent, GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                AssetDatabase.OpenAsset(Asset);
            }
        }
        void OpenScene(SceneAsset scene)
        {
            if (scene != null)
            {
                string scenePath = AssetDatabase.GetAssetPath(scene);
                if (!string.IsNullOrEmpty(scenePath))
                {
                    EditorSceneManager.OpenScene(scenePath);
                }
            }
        }
        public override string ToString()
        {
            if (Asset == null)
            {
                return string.Empty;
            }
            return Asset.name;
        }
    }
}
