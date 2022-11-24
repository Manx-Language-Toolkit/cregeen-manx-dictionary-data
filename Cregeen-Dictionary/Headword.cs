using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cregeen;

internal class Headword
{
    public Headword(Definition def)
    {
        Definition = def;
        Definition.Depth = -1;
    }

    public Definition Definition { get; }

    public IEnumerable<Definition> All => Definition.AllChildren;

    internal static Headword? FromHtml(HtmlNode arg1, int arg2)
    {
        // &nbsp; was the content, or a section heading: "A"
        if (arg1.InnerText.Length == 1 || string.IsNullOrWhiteSpace(HttpUtility.HtmlDecode(arg1.InnerText)) || arg1.GetAttributes().Any(x => x.Name == "class" && x.Value.Contains("do-not-translate")))
        {
            return null;
        }

        return FromHtmlUnsafe(arg1.InnerHtml);
    }

    internal static Headword FromHtmlUnsafe(string html)
    {
        List<Definition> splitOnLineBreak = ConvertToDefinitions(html);

        var headword = splitOnLineBreak[0];

        var toReturn = new Headword(headword);

        var children = splitOnLineBreak.Skip(1).ToList();

        var depths = children.Select(x => x.Depth).Distinct().OrderBy(x => x).ToList();

        var wordToDepth = splitOnLineBreak.ToDictionary(x => x, x => depths.IndexOf(x.Depth));

        foreach (var (node, pos) in splitOnLineBreak.Select((x, i) => (x, i)).Skip(1))
        {
            // find the depth
            var depth = wordToDepth[node];
            for (int j = pos - 1; j >= 0; j--)
            {
                var parent = splitOnLineBreak[j];
                var parentDepth = wordToDepth[parent];
                if (parentDepth < depth)
                {
                    parent.AddChild(node);
                    break;
                }
            }
        }

        return toReturn;
    }

    internal static List<Definition> ConvertToDefinitions(string html)
    {
        // Note: We change <br> to <br/> for an explicit newline
        var lines = html.Split("<br>");
        var definitions = lines.Where(x => !string.IsNullOrWhiteSpace(x)).Select(Definition.FromHtml).ToList();
        
        // Handle ".id" in the text == Same As Above
        foreach (var (definition, i) in definitions.Select((x,i) => (x,i)))
        {
            if (!definition.Abbreviations.Contains(Abbreviation.TheSameAsAbove))
            {
                continue;
            }

            if (i == 0)
            {
                Console.WriteLine("warn: found '.id' in first line: " + definition.Word); // + ":" + definition.Entry);
                continue;
            }

            if (!SkippableEntryText.Contains(definition.EntryText.Trim()))
            {
                Console.WriteLine("warn: unexpected entry text:" + definition.Word + ":" + definition.EntryText);
                definition.EntryTextOverride = definition.EntryText + "; " + definitions[i - 1].EntryText;
            }
            else
            {
                definition.EntryTextOverride = definitions[i - 1].EntryText;
            }
        }

        return definitions;
    }

    private static readonly List<string> SkippableEntryText = new()
    {
        "",
        "A",
        "161.",
        "[F]",
        ", 58.",
        ",] 58.",
        "], 58.",
        ",] 58.",
        ",] 58. E",
        ",] 58. M",
        ",] 58. F",
        ",] 58. F.",
        ",] 58. L",
        ",] 58. K",
        ",] 58. M",
        ",] 58. V",
        ",] 58. W",
        ",] 58. J",
        ",] 58. G",
        ",] 58. R",
        ",] 58. P",
        ",] 58. O",
        ",] 58. T",
        "E",
        "O",
        "F"
    };
}