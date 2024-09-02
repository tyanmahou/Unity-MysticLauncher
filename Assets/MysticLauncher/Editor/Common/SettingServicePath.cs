using System;
using UnityEditor;

namespace Mystic
{
    /// <summary>
    /// セッティングパス
    /// </summary>
    [Serializable]
    public struct SettingServicePath
    {
        public SettingsScope Scope;
        public string SettingPath;
    }
}
