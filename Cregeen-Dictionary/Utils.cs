using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using HtmlAgilityPack;

namespace Cregeen;

public static class Utils
{
    [Pure]
    public static IList<string> GetBoldWords(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        return doc.DocumentNode.Descendants("b").Select(x => x.InnerText).ToList();
    }

    public static string TrimBraces(string data)
    {
        if (!data.Contains("&lt;"))
        {
            return data;
        }

        var firstIndex = data.IndexOf("&lt;", StringComparison.Ordinal);
        if (firstIndex != data.LastIndexOf("&lt;", StringComparison.Ordinal))
        {
            throw new NotImplementedException("more than one element");
        }
        var endIndex = data.IndexOf("&gt;", StringComparison.Ordinal);

        return data[..(firstIndex)] + data[(endIndex + "&gt;".Length)..];
    }
}