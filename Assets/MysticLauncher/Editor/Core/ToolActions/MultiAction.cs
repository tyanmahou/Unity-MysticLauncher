using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class MultiAction : IToolAction
    {
        [SerializeReference, SubclassSelector]
        public IToolAction[] Actions = new IToolAction[0];

        public void Execute()
        {
            foreach (var action in Actions) 
            {
                action?.Execute();
            }
        }
        public string Tooltip()
        {
            return string.Join(Environment.NewLine, Actions.Select(a => a?.Tooltip()).Where(s => !string.IsNullOrEmpty(s)));
        }
    }
}
