using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Mystic
{
    public static class HistoryTracker
    {
        [OnOpenAsset]
        public static bool OnOpen(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);

            UserHistories.instance.Register(asset);
            UserHistories.instance.Save();
            return false;
        }
    }
}
