using Mystic;
using UnityEditor;
using UnityEngine;

public class MyElement : IElement
{
    public void OnGUI()
    {
        EditorGUILayout.LabelField("Log");
        _text = EditorGUILayout.TextArea(_text);
        if (GUILayout.Button("Submit"))
        {
            Debug.Log(_text);
        }
    }
    string _text = string.Empty;
}
