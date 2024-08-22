using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mystic
{
	public class LauncherPreferenceSettingsProvider : SettingsProvider
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
			return new LauncherPreferenceSettingsProvider(SettingPath, SettingsScope.User, keywords);
		}

		public LauncherPreferenceSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords)
		{
		}
		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			var preferences = LauncherPreferenceSettings.instance;
			preferences.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
			Editor.CreateCachedEditor(preferences, null, ref _editor);
		}

		public override void OnGUI(string searchContext)
		{
			EditorGUI.BeginChangeCheck();
			// 設定ファイルの標準のインスペクターを表示
			_editor.OnInspectorGUI();
			if (EditorGUI.EndChangeCheck())
			{
				// 差分があったら保存
				LauncherPreferenceSettings.instance.Save();
			}
		}
	}
}
