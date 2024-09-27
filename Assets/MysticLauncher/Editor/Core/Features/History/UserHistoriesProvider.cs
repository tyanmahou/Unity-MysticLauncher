using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mystic
{
    public class UserHistoriesProvider : SettingsProvider
    {
        private Editor _editor;
        public const string SettingPath = "Preferences/Mystic Launcher/History";

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            string[] keywords = new string[]
            {
                "Launcher"
            };
            return new UserHistoriesProvider(SettingPath, SettingsScope.User, keywords);
        }

        public UserHistoriesProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords)
        {
        }
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var preferences = UserHistories.instance;
            preferences.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
            Editor.CreateCachedEditor(preferences, null, ref _editor);
        }

        public override void OnGUI(string searchContext)
        {
            _editor.OnInspectorGUI();
        }
    }
}
