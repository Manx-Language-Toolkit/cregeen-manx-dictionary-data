namespace Cregeen
{
    public static class Extensions
    {
        public static string TrimAfter(this string target, params string[] suffix)
        {
            foreach (var s in suffix)
            {
                var index = target.IndexOf(s);
                if (index == -1)
                {
                    continue;
                }
                return target.Substring(0, index);
            }

            return target;
        }
    }
}
