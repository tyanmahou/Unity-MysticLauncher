using UnityEngine;

namespace Mystic
{
    internal static class VersionUtil
    {
        public static System.Version UnityVersion => new System.Version(ParseVersion(Application.unityVersion));

        public static bool IsNewerThan(string unityVersion) => UnityVersion > new System.Version(ParseVersion(unityVersion));

        private static string ParseVersion(string unityVersion)
        {
            // "6000.3.8f1" → "6000.3.8"
            int i = unityVersion.IndexOfAny(new char[] { 'a', 'b', 'f', 'p' });
            return i > 0 ? unityVersion.Substring(0, i) : unityVersion;
        }
    }
}
