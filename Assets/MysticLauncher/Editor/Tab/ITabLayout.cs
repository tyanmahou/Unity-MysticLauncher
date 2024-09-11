namespace Mystic
{
    public interface ITabLayout
    {
        public string Title { get; }
        public Icon Icon { get;}
        void OnGUI();
    }
}
