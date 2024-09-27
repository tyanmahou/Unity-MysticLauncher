using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [FilePath("UserSettings/MysticUserEnv.asset", FilePathAttribute.Location.ProjectFolder)]
    public class UserEnv : ScriptableSingleton<UserEnv>
    {
        [SerializeField, FileSelect]
        private string _terminalPath = string.Empty;
        public string TerminalPath => _terminalPath;

        [SerializeField]
        private EnvVariable[] _variables = new EnvVariable[0];

        public string GetVariable(string variable)
        {
            foreach (var v in _variables)
            {
                if (v.Variable == variable)
                {
                    return v.Value;
                }
            }
            string envVar = Environment.GetEnvironmentVariable(variable);
            if (string.IsNullOrEmpty(envVar))
            {
                return variable;
            }
            return envVar;
        }

        public string Replace(string value)
        {
            // 環境変数 $(VARIABLE_NAME) を置換
            const string pattern = @"\$\((.*?)\)";

            return Regex.Replace(value, pattern, match =>
            {
                string variableName = match.Groups[1].Value;
                string value = GetVariable(variableName);
                return !string.IsNullOrEmpty(value) ? value : match.Value;
            });
        }

        public void Save()
        {
            Save(true);
        }
        private void OnValidate()
        {
            if (!EditorUtility.IsPersistent(this))
            {
                Save(true);
            }
        }
    }
}
