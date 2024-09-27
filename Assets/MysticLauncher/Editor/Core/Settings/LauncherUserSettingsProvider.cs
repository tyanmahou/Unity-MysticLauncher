using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mystic
{
    public class LauncherUserSettingsProvider : SettingsProvider
    {
        private Editor _editor;
        public const string SettingPath = "Preferences/Mystic Launcher";

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            string[] keywords = new string[]
            {
                "Launcher"
            };
            return new LauncherUserSettingsProvider(SettingPath, SettingsScope.User, keywords);
        }

        public LauncherUserSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords)
        {
        }
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var preferences = LauncherUserSettings.instance;
            preferences.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
            Editor.CreateCachedEditor(preferences, null, ref _editor);
        }

        public override void OnGUI(string searchContext)
        {
            _editor.OnInspectorGUI();
        }
    }
}
