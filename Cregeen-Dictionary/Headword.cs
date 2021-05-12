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
        }

        public Definition Definition { get; }

        public IEnumerable<Definition> All => Definition.All;

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

            toReturn.AddChildren(splitOnLineBreak);

            return toReturn;
        }

        private void AddChildren(List<Definition> splitOnLineBreak)
        {

            Definition.Children.AddRange(splitOnLineBreak.Skip(1));

            foreach (var child in Definition.Children)
            {
                child.Headword = this;
            }
        }
    }
}