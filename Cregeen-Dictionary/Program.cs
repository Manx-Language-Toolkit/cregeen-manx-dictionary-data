using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cregeen
{

    /**
     * This is a quick script which should take the following HTML document and convert it to a JSON output containing:
     * A tree of entries
     *    * With suffixes expanded to contain all words
     *      *  ynseydagh [change -agh to -ee] => ["ynseydagh", "ynseydee"]
     *    * And the HTML to display each entry
     *
     *  This document has been manually modified
     *  <br> has been converted to <p></p> to avoid detecting the <br> as a new word
     *  commas were added after definitions
     *  certain words were expanded (deinagh(ey) -> deinagh or deinaghey)
     *
     *  Further steps:
     *  * Parse the HTML to provide more context for the word (plural, adjective, etc...). We do not need the stress, as this is content
     *  * Handle 3-level nestings: "aa-" => "aa-chionnagh" => "aa-chionnit"
     */
    class Program
    {

        private static string FIRST_WORD = "aa"; // technically: aa‑, but the non-breaking hyphen causes issues
        private static string LAST_WORD = "yskid";

        static void Main(string[] args)
        {
            // This code is fairly lazy - main improvements would be to extract the 'verb/noun' into structured text, as well as the pronunciation.

            string resourceName = "Cregeen.aa-orderit.01052020-filtered.htm";

            var docText = LoadWordEncodedFile(resourceName);

            docText = docText.Replace("<i><br>\r\n", "<br>\r\n<i>");

            var doc = new HtmlDocument();
            doc.LoadHtml(docText);


            var headwords = doc.DocumentNode.Descendants("p")
                .Skip(1708)  // skip the preamble
                .Take(3451)  // and suffix
                .Select(Headword.FromHtml)
                .Where(x => x != null)
                .ToList();

            VerifyHeadwords(headwords);

            var allWordDefinitions = headwords.SelectMany(x => x.All);

            HashSet<string> ok = new HashSet<string>()
            {
                "yn niagh [sc. yn eagh]",
                "lus ny chroshey (sic)",
                "yn cherçheen (sic: stress)",
                "e vanisthie (sic: stress)",
                "e vouyranys (sic: stress)",
                "nyn moghlane (sic: stress)",
                "yn vless (sic: sc. vlest ?)",
                "berçhee (sic: sc. berçhagh)",
                "yn çhennar (sic: stress)",
                "cha ghleayn (sic: gleayn)",
                "er ny gooilleeney (sic)",
                "driualtys [l. druailtys ?]",
                "e aasaag (sic: stress)",
                "dy ailleil (sic: stress)",
                "nyn gialgeyrys (sic: stress)",
                "nyn brendeys (sic: stress)",
                "nyn bundail (sic: stress)",
                "e halmane (sic: stress)",
                "e heebane (sic: stress)",
                "nyn gheh (sic: see nyn jeh)",
                "yn çhelgeyr (sic: stress)",
                "ny hideyr (sic: stress)",
                "neu-çhaglit (sic: l. neu-haglit ?)",
                "yn chraiuaig (sic: stress)",
                "nyn jymmyltagh [sic: sc. ny hymmyltagh",
                "er ny gleayney (sic)",
                "yn chonvayrt (sic: stress)",
                "dy oardrail (sic: stress)",
                "screeueyrys [l. screeudeyrys ?]",
                "yn çheeloghe (sic: stress)",
                "dy oardrail (sic: stress)",
                "bare lhieusyn <or lhieuish>",
                "",
                "",
                "",




                "",
                "",
                "",
                "sheain eh mie orrin",
                "re-hollys vooar y n’ouyr",
                "yn wheig as feedoo",
                "e veeghyn dy hymmey",
                "jeih thousanyn as feed",
                "foddey er dy henney",
                "agh son shoh as ooilley",
                "eh ta dy my choyrt",
                "er-çhee dy yannoo",
                "lus ny binjey lheeanagh",
                "lus ny binjey mooar",
                "shee dy row marin",
                "shee dy row mayrt",
                "shee dy row meriu",
                "shee dy row hiu",
                "shee dy vea dty valley",
                "mygeayrt y mysh",
                "mygeayrt y mo’ee",
                "mygeayrt y moo",
                "mygeayrt y moom",
                "mygeayrt y mooin",
                "mygeayrt y mood",
                "mygeayrt y miu",
                "lus ny moyl Moirrey",
                "lus feie y tooill",
                "lus millish ny lheeanagh",
                "lus villish ny lheeanagh",
                "lus ny freenaghyn mooarey",
                "lus ny moal moirrey",
                "lus ny moyl moirrey",
                "lus y çhengey veg",
                "lus y cramman doo",
                "lus y daa phing",
                "fer loayrt as lheh",
                "fer loayrt er nyn son",
                "my ta dy gha",
                "my va dy gha",
                "my ny gione",
                "ta shen dy ghra",
                "ny veggan as ny veggan",
                "re-hollys vooar ny gabbyl",
                "dy voddey beayn y ree",
                "as hrog ad orroo",
                "quoi ec ta fys",
                "cha vurrys lhiam da",
                "cha burrys lhiam da jannoo eh",
                "er-y-traa t'ayn ta lhie yn stayd beayn ain",
                "er mooin y cheilley",
                "Laa’l Moirrey ny Gianle",
                "nagh lhig y Jee",
                "my veelley mhillee ort",
                "lus ny moal Moirrey",
                "er nyn skyn",
                "lus y chramman doo",
                "er-y-traa t'ayn ta lhie yn stayd beayn ain.",
                "goll er mullagh ching",
                "gur eh mie eu",
                "gow hood hene eh",
                "inneenyn braar as shuyr",
                "kiare-feed as nuy persoonyn jeig",
                "ushag roauyr ny hoarn",
                "nane jeig as feed",
                "feer vun ry skyn",
                "as haink eh gy kione",
                "Lhuingys Chaggee Reeoil Hostyn",
                "scarrey veih yn agglish",
                "twoaie as gys y sheear",
                "son shen as ooilley",
                "y ghaddee myr t’ou",

                // probably not OK
                "nyn <maase or> maash",
                "e <gheul or> gheuley",
            };

            List<string> maybeInvalid = new List<string>();
            List<string> withParen = new List<string>();

            // Obtain the "bad" input to display in-console for manual fixes of the .htm
            foreach (Definition def in allWordDefinitions)
            {
                bool ContainsMoreThan2Words(Definition def) => def.Word.Count(x => x == ' ') > 2;
                bool IsAllowListed(Definition def) => ok.Contains(def.Word.Trim());

                if (ContainsMoreThan2Words(def) && !def.Word.Contains(" or ") && !IsAllowListed(def))
                {
                    maybeInvalid.Add(def.Word);
                }

                var main = string.Join("\n", def.PossibleWords);

                if (main.Contains("(") && !main.Contains("stress") && !main.Contains("sic") && !main.Contains("(sc"))
                {
                    withParen.Add(main);
                }
            }

            // Display the issues
            Console.WriteLine();
            foreach (var v in withParen.Concat(maybeInvalid))
            {
                Console.WriteLine(v.Trim());
            }

            // Write the JSON to a file
            var directory = Path.Combine(Environment.CurrentDirectory, "Output");
            var outPath = Path.Combine(directory, "cregeen-v1.json");
            Console.WriteLine($"Writing to {outPath}");
            Directory.CreateDirectory(directory);
            
            using (StreamWriter sw = new StreamWriter(new FileStream(outPath, FileMode.Create), Encoding.UTF8))
            {
                sw.WriteLine(JsonConvert.SerializeObject(headwords.Select(headword => OutDef.FromDef(headword)), Json.JsonSettings));
            }
        }

        private static void VerifyHeadwords(List<Headword> headwords)
        {
            if (!headwords.First().Definition.Word.StartsWith(FIRST_WORD))
            {
                throw new InvalidOperationException($"data is missing. Expected: {FIRST_WORD}. Got: {headwords.First().Definition.Word}");
            }

            if (!headwords.Last().Definition.Word.StartsWith(LAST_WORD))
            {
                throw new InvalidOperationException($"data is missing. Expected: {LAST_WORD}. Got: {headwords.Last().Definition.Word}");
            }
        }

        private static string LoadWordEncodedFile(string resourceName)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Encoding wind1252 = Encoding.GetEncoding(1252);
            // 1252 encoded :/
            return File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Resources", resourceName), wind1252);
        }
    }

    public class OutDef
    {
        public string[] Words { get; set; }
        public string EntryHtml { get; set; }
        public string HeadingHtml { get; set; }
        public OutDef[] Children { get; set; }

        internal static OutDef FromDef(Headword def)
        {
            return FromDef(def.Definition);
        }

        internal static OutDef FromDef(Definition def)
        {
            return new OutDef
            {
                Words = def.PossibleWords.ToArray(),
                EntryHtml = FixUnclosedTags(def.Extra),
                HeadingHtml = FixUnclosedTags(def.Heading),
                Children = def.Children.Select(FromDef).ToArray()
            };
        }

        private static string FixUnclosedTags(string heading)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(heading);
            return document.DocumentNode.OuterHtml;
        }
    }
}
