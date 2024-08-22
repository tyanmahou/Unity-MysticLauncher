using UnityEditor;
using UnityEngine;

namespace Mystic
{
	[FilePath("UserSettings/MysticLauncherPreferencesSettings.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class LauncherPreferenceSettings : ScriptableSingleton<LauncherPreferenceSettings>
	{
		[SerializeField]
		private string _terminalPath = "";
		public string TerminalPath => _terminalPath;


		public void Save()
		{
			Save(true);
		}
	}
}
