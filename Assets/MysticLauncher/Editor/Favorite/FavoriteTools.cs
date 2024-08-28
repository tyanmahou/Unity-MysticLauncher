using UnityEditor;

namespace Mystic
{
    /// <summary>
    /// お気に入りツール
    /// </summary>
    public static class FavoriteTools
    {
        private static FavoriteList Instance => LauncherUserSettings.instance.Favorite;
        [MenuItem("Assets/Mystic/Favorite")]
        private static void Favorite()
        {
            FavoriteWindow.Show(Selection.activeObject);
        }

        [MenuItem("Assets/Mystic/Favorite", true)]
        private static bool FavoriteValidation()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }
            return !Instance.IsRegistered(Selection.activeObject);
        }

        [MenuItem("Assets/Mystic/Unfavorite")]
        private static void Unavorite()
        {
            Instance.Unregister(Selection.activeObject);
        }


        [MenuItem("Assets/Mystic/Unfavorite", true)]
        private static bool UnfavoriteValidation()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }
            return Instance.IsRegistered(Selection.activeObject);
        }
    }
}
