using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mystic
{
    public class LauncherProjectSettingsProvider : SettingsProvider
    {
        private Editor _editor;
        public const string SettingPath = "Project/Mystic Launcher";

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            string[] keywords = new string[]
            {
                "Launcher"
            };
            return new LauncherProjectSettingsProvider(SettingPath, SettingsScope.Project, keywords);
        }

        public LauncherProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords)
        {
        }
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var preferences = LauncherProjectSettings.instance;
            preferences.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
            Editor.CreateCachedEditor(preferences, null, ref _editor);
        }

        public override void OnGUI(string searchContext)
        {
            _editor.OnInspectorGUI();
        }
    }
}
