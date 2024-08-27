using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// Pathユーティリティ
    /// </summary>
    public static class PathUtil
    {
        public static string RelativeOrFullPath(string path)
        {
            return System.IO.Path.IsPathRooted(path)
                ? path
                : Application.dataPath + "/../" + path;
        }
    }
}
