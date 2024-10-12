using UnityEditor;

namespace Mystic
{
    public static class MysticResource
    {
        public static T LoadPackageAsset<T>(string path)
            where T : UnityEngine.Object
        {
            var ret = AssetDatabase.LoadAssetAtPath<T>("Packages/com.tyanmahou.mystic-launcher/Resources/" + path);
            if (ret is null)
            {
                ret = AssetDatabase.LoadAssetAtPath<T>("Assets/MysticLauncher/Resources/" + path);
            }
            return ret;
        }
    }
}
