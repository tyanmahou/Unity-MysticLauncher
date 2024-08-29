using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

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

            GUIStyle buttonStyle = new GUIStyle(EditorStyles.objectField);
            buttonStyle.margin.left += EditorGUI.indentLevel * 15;

            if (GUILayout.Button(content, buttonStyle, GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                AssetDatabase.OpenAsset(Asset);
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("ViewToolZoom On@2x"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                EditorGUIUtility.PingObject(Asset);
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
