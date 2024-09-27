using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;

namespace Mystic
{
    [InitializeOnLoad]
    public static class HistoryTracker
    {
        static HistoryTracker()
        {
            PrefabStage.prefabStageOpened += OnPrefabOpened;
        }
        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);
            Register(asset);
            return false;
        }
        private static void OnPrefabOpened(PrefabStage stage)
        {
            UnityEngine.GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(stage.assetPath);
            Register(prefabAsset);
        }

        private static void Register(UnityEngine.Object obj)
        {
            if (obj != null)
            {
                UserHistories.instance.Register(obj);
                UserHistories.instance.Save();
            }
        }
    }
}
