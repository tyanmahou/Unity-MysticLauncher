using System;
using System.Configuration;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

namespace Mystic
{
    /// <summary>
    /// Pathユーティリティ
    /// </summary>
    public static class PathUtil
    {
        public static string ProjectPath => Application.dataPath + "/../";
        public static string FixedPath(string path)
        {
            return RelativeOrFullPath(ReplaceEnv(path ?? string.Empty));
        }
        public static string RelativeOrFullPath(string path)
        {
            return System.IO.Path.IsPathRooted(path)
                ? path
                : ProjectPath + path;
        }
        public static string RelativePathInProject(string fullPath, bool isDirectory = false)
        {
            return RelativePathInProject(ProjectPath, fullPath ?? string.Empty, isDirectory);
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

        /// <summary>
        /// 環境変数置換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceEnv(string value)
        {
            return LauncherUserSettings.instance.Env.Replace(value);
        }
    }
}
