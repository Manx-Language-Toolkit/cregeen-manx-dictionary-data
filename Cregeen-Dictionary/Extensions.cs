using System;

namespace Cregeen;

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

    public static string RemoveBetween(this string target, string start, string end)
    {
        // PERF: Inefficient - 
        var index = target.IndexOf(start);
        while (index != -1)
        {
            var endIndex = target.IndexOf(end, index) + end.Length;
            if (endIndex == 0)
            {
                break;
            }
            if (endIndex <= index)
            {
                throw new ArgumentException();
            }
            target = target.Substring(0, index) + target.Substring(endIndex);


            index = target.IndexOf(start);
        }
        return target;
    }
}