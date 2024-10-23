using System.Collections.Generic;
using System;
using UnityEngine;
using System.Reflection;
using UnityEditor;
using System.Linq;

namespace Mystic
{
    /// <summary>
    /// UnityIconユーティリティ
    /// </summary>
    public static class UnityIconUtil
    {
        public static string[] GetIconNames()
        {
            return _iconNames ??= EnumerateIconNames().Distinct().ToArray();
        }
        private static IEnumerable<string> EnumerateIconNames()
        {
            AssetBundle bundle = GetEditorAssetBundle();
            foreach (var assetName in EnumerateIconAssetNames(bundle))
            {
                var icon = bundle.LoadAsset<Texture2D>(assetName);
                if (icon == null)
                {
                    continue;
                }
                yield return icon.name;
            }
        }
        private static IEnumerable<string> EnumerateIconAssetNames(AssetBundle bundle)
        {
            string iconsPath = UnityEditor.Experimental.EditorResources.iconsPath;
            foreach (var assetName in bundle.GetAllAssetNames())
            {
                if (assetName.StartsWith(iconsPath, StringComparison.OrdinalIgnoreCase) == false)
                {
                    continue;
                }
                if (assetName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) == false &&
                    assetName.EndsWith(".asset", StringComparison.OrdinalIgnoreCase) == false)
                {
                    continue;
                }

                yield return assetName;
            }
        }
        private static AssetBundle GetEditorAssetBundle()
        {
            var getEditorAssetBundle = typeof(EditorGUIUtility).GetMethod(
                "GetEditorAssetBundle",
                BindingFlags.NonPublic | BindingFlags.Static
                );
            return (AssetBundle)getEditorAssetBundle.Invoke(null, new object[] { });
        }
        static string[] _iconNames = null;
    }
}
