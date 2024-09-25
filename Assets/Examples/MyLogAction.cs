using Mystic;
using UnityEngine;

public class MyLogAction : IToolAction
{
    [SerializeField] string _text;

    public void Execute()
    {
        Debug.Log(_text);
    }
}
