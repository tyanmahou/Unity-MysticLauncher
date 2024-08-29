using System;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// Pathユーティリティ
    /// </summary>
    public static class PathUtil
    {
        public static string ProjectPath => Application.dataPath + "/../";
        public static string RelativeOrFullPath(string path)
        {
            return System.IO.Path.IsPathRooted(path)
                ? path
                : ProjectPath + path;
        }
        public static string RelativePathInProject(string fullPath, bool isDirectory = false)
        {
            return RelativePathInProject(ProjectPath, fullPath, isDirectory);
        }
        public static string RelativePathInProject(string basePath, string fullPath, bool isDirectory = false)
        {
            Uri baseUri = new Uri(basePath.EndsWith("/") ? basePath : basePath + "/");
            if (isDirectory)
            {
                fullPath = fullPath.EndsWith("/") ? fullPath : fullPath + "/";
            }
            Uri fullUri = new Uri(fullPath);
            if (baseUri.AbsolutePath == fullUri.AbsolutePath)
            {
                // 同じフォルダ
                return "./";
            }
            else if (baseUri.IsBaseOf(fullUri))
            {
                // 相対パス
                Uri relativeUri = baseUri.MakeRelativeUri(fullUri);
                return "./" + relativeUri.ToString();
            }
            else
            {
                // 絶対パス
                return fullPath;
            }
        }
    }
}
