using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
	[FilePath("ProjectSettings/MysticLauncherProjectSettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public class LauncherProjectSettings : ScriptableSingleton<LauncherProjectSettings>
	{
		public void Save()
		{
			Save(true);
		}
	}
}
