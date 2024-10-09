using System;
using UnityEditor;

namespace Mystic
{
    [Serializable]
    public class OpenAssetAction : IToolAction
    {
        public UnityEngine.Object Asset;

        public void Execute()
        {
            AssetDatabase.OpenAsset(Asset);
        }
        public string Tooltip()
        {
            if (Asset == null)
            {
                return string.Empty;
            }
            var path = AssetDatabase.GetAssetPath(Asset);
            return $"Open Asset\n<color=grey>{path}</color>";
        }
    }
}
