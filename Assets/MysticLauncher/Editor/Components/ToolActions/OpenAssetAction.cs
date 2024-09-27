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
    }
}
