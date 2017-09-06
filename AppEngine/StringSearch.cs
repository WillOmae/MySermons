using System.Linq;

namespace AppEngine
{
    public class StringSearch
    {
        public static bool AllExist(string haystack, string needle)
        {
            bool hasFoundAll = false;

            foreach (char unit in needle.ToLower())
            {
                if (haystack.Contains(unit))
                {
                    hasFoundAll = true;
                }
                else
                {
                    hasFoundAll = false;
                    break;
                }
            }
            return hasFoundAll;
        }
        public static bool AllExist_InOrder(string haystack, string needle)
        {
            bool hasFoundAll = false;

            foreach (char unit in needle)
            {
                if (haystack.Contains(unit))
                {
                    hasFoundAll = true;
                    int index = haystack.IndexOf(unit);
                    haystack = haystack.Remove(0, index + 1);
                }
                else
                {
                    hasFoundAll = false;
                    break;
                }
            }
            return hasFoundAll;
        }
    }
}
