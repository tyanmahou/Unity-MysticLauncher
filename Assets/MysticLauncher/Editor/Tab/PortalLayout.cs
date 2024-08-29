using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class PortalLayout : ITabLayout
    {
        public string Title => "Portal";
        public Icon Icon { get; set; } = Icon.CreateUnityIcon("d_Profiler.UIDetails");

        public void OnGUI()
        {
            var elements = LauncherProjectSettings.instance.PortalLayout;
            if (elements.Length <= 0)
            {
                EditorGUILayout.HelpBox("Custom Edit Project Portal Page", MessageType.Info);
                var icon = new GUIContent(EditorGUIUtility.IconContent("d__Popup"));
                icon.text = "Edit";
                if (GUILayout.Button(icon))
                {
                    SettingsService.OpenProjectSettings(LauncherProjectSettingsProvider.SettingPath);
                }
                return;
            }
            foreach (var entry in elements)
            {
                entry?.OnGUI();
            }
        }
        public override string ToString()
        {
            return Title;
        }
    }
}
