
namespace Mystic
{
    /// <summary>
    /// string拡張
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsSearched(this string str, string filter)
        {
            return str.IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
