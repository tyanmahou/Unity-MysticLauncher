using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// 環境変数設定
    /// </summary>
    [Serializable]
    public class EnvSettings
    {
        [Serializable]
        struct VariableType
        {
            public string Variable;
            public string Value;
        }
        [SerializeField]
        private VariableType[] _variables = new VariableType[0];

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
    }
}
