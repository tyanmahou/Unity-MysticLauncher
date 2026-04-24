using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class TimeScalePopup : PopupWindowContent
    {

        public override Vector2 GetWindowSize()
        {
            return new Vector2(300, EditorGUIUtility.singleLineHeight + 5);
        }

        public override void OnOpen()
        {
        }

        public override void OnGUI(Rect rect)
        {
            using var _ = new EditorGUI.DisabledScope(!Application.isPlaying);

            EditorGUI.BeginChangeCheck();

            var timeScale = EditorGUILayout.FloatField("TimeScale", Time.timeScale);
            if (EditorGUI.EndChangeCheck())
            {
                Time.timeScale = timeScale;
            }
        }
    }
}
