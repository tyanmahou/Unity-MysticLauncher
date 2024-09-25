using Mystic;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ScriptsLayout : ITabLayout
{
    public string Title => "Scripts";

    public Icon Icon => Icon.CreateUnityIcon("cs Script Icon");

    public void OnGUI()
    {
        _searchString = EditorGUILayout.TextField(GUIContent.none, _searchString, EditorStyles.toolbarSearchField);
        _scripts ??= FindScripts();
        EditorGUIUtil.DrawSeparator();

        using (var scroller = new EditorGUILayout.ScrollViewScope(_scroll))
        {
            foreach (var script in _scripts
                .Where(s => s.name.IndexOf(_searchString, System.StringComparison.OrdinalIgnoreCase) >= 0))
            {
                EditorGUILayout.ObjectField(script, typeof(MonoScript), false);
            }
            _scroll = scroller.scrollPosition;
        }
    }
    IReadOnlyList<MonoScript> FindScripts()
    {
        List<MonoScript> result = new List<MonoScript>();
        // 全てのMonoScriptアセットを検索
        string[] guids = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            if (script != null)
            {
                result.Add(script);
            }
        }
        return result;
    }
    string _searchString = string.Empty;
    IReadOnlyList<MonoScript> _scripts;
    Vector2 _scroll;
}
