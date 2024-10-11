using UnityEngine;

namespace Mystic
{
    [CreateAssetMenu(fileName = "NewTemplateLayout", menuName = "Mystic/TemplateLayoutAsset", order =99999)]
    public class TemplateLayoutAsset : ScriptableObject
    {
        public Label Title;
        [NamedArrayElement, SerializeReference, SubclassSelector]
        public IElement[] Elements;
    }
}
