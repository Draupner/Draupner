namespace Scaffold.Common
{
    public class StringHelper
    {
        public static string LowercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToLower(a[0]);
            return new string(a);
        }
    }
}
