using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cregeen
{
    internal class Headword
    {
        public Headword(Definition def)
        {
            this.Definition = def;
            this.Definition.Depth = -1;
        }

        public Definition Definition { get; }

        public IEnumerable<Definition> All => Definition.AllChildren;

        internal static Headword FromHtml(HtmlNode arg1, int arg2)
        {
            // &nbsp; was the content, or a section heading: "A"
            if (arg1.InnerText.Length == 1 || string.IsNullOrWhiteSpace(HttpUtility.HtmlDecode(arg1.InnerText)) || arg1.GetAttributes().Any(x => x.Name == "class" && x.Value.Contains("do-not-translate")))
            {
                return null;
            }

            // Note: We change <br> to <br/> for an explicit newline
            var splitOnLineBreak = arg1.InnerHtml.Split("<br>").Select(x => Definition.FromHtml(x)).ToList();

            var headword = splitOnLineBreak[0];

            var toReturn = new Headword(headword);

            var children = splitOnLineBreak.Skip(1).ToList();

            var depths = children.Select(x => x.Depth).Distinct().OrderBy(x => x).ToList();

            var wordToDepth = splitOnLineBreak.ToDictionary(x => x, x => depths.IndexOf(x.Depth));
            
            foreach (var (node, pos) in splitOnLineBreak.Select((x, i) => (x,i)).Skip(1))
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
    }
}