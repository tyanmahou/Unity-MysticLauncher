using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Mystic
{
    class ToggleAnimBool
    {
        public ToggleAnimBool(bool isOn)
        {
            _anim = new AnimBool(isOn);
            _anim.valueChanged.RemoveAllListeners();
            _anim.valueChanged.AddListener(() =>
            {
                if (EditorWindow.focusedWindow != null)
                {
                    EditorWindow.focusedWindow.Repaint();
                }
            });
        }
        public bool IsOn
        {
            get => _anim.target;
            set => _anim.target = value;
        }
        public float Faded => _anim.faded;

        private AnimBool _anim;
    }
}
