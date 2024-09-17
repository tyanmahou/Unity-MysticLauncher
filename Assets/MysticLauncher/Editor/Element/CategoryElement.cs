using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;

namespace Mystic
{
    [Serializable]
    public class CategoryElement : IElement
    {
        public static CategoryElement Create(string text, string tooltip = "", string icon = null)
        {
            return new CategoryElement()
            {
                Label = Label.Create(text, tooltip, icon),
            };
        }
        const float height = 30;
        const float iconSize = 20;

        public Label Label;

        [NamedArrayElement, SerializeReference, SubclassSelector]
        public IElement[] Elements;

        public void OnGUI()
        {
            GUILayout.Space(4);
            DrawHeader();

            _fade.target = _isOpen;
            if (EditorGUILayout.BeginFadeGroup(_fade.faded))
            {
                GUILayout.Space(2);
                DrawContent();
            }
            EditorGUILayout.EndFadeGroup();
        }
        private void DrawHeader()
        {
            var style = new GUIStyle(EditorStyles.helpBox);
            style.margin.left += EditorGUI.indentLevel * 15;
            Rect buttonRect = GUILayoutUtility.GetRect(0, height, style);

            if (buttonRect.Contains(Event.current.mousePosition))
            {
                EditorGUI.DrawRect(buttonRect, new Color(0, 1, 1, 0.1f));
            }

            Rect contentRect = buttonRect;
            contentRect.x += 5;
            contentRect.y += 5;
            contentRect.height = iconSize;

            if (Label.Icon.TryGetGUIContent(out var content))
            {
                contentRect.width = iconSize;
                GUI.DrawTexture(contentRect, content.image, ScaleMode.ScaleToFit);

                contentRect.x += contentRect.width + 5;
                contentRect.width = buttonRect.width - contentRect.width - 15;

                GUI.Label(contentRect, Label.Text, EditorStyles.boldLabel);
            }
            else
            {
                contentRect.width = buttonRect.width - 10;

                GUI.Label(contentRect, Label.Text, EditorStyles.boldLabel);
            }
            string status = _isOpen ? "－" : "＋";
            contentRect.x += contentRect.width;
            contentRect.x -= 20;
            contentRect.width = 20;
            GUI.Label(contentRect, status, EditorStyles.boldLabel);

            if (GUI.Button(buttonRect, new GUIContent(string.Empty, Label.Tooltip), style))
            {
                _isOpen = !_isOpen;
                _fade.valueChanged.RemoveAllListeners();
                _fade.valueChanged.AddListener(() =>
                {
                    if (EditorWindow.focusedWindow != null)
                    {
                        EditorWindow.focusedWindow.Repaint();
                    }
                });
            }
        }
        private void DrawContent()
        {
            foreach (var entry in Elements)
            {
                if (entry is CategoryElement)
                {
                    using var indent = new EditorGUI.IndentLevelScope();
                    entry?.OnGUI();
                }
                else
                {
                    entry?.OnGUI();
                }
            }
        }
        public override string ToString()
        {
            return Label.Text;
        }
        AnimBool _fade = new AnimBool();
        bool _isOpen = true;
    }
}
