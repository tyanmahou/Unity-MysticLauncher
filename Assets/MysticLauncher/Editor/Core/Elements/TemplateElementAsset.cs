using UnityEngine;

namespace Mystic
{
    [CreateAssetMenu(fileName = "NewTemplateElement", menuName = "Mystic/TemplateElementAsset", order=99998)]
    public class TemplateElementAsset : ScriptableObject
    {
        [NamedArrayElement, SerializeReference, SubclassSelector]
        public IElement[] Elements;
    }
}
