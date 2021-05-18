using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Cregeen
{
    [DebuggerDisplay("{originalWord}")]
    public class Definition
    {
        /// <summary>
        /// Html word
        /// </summary>
        private string originalWord;
        /// <summary>
        /// Html Extra
        /// </summary>
        private readonly string originalExtra;


        /// <summary>
        /// Decoded word
        /// </summary>
        public string Word { get; }
        public string Extra { get; }

        public string Entry => DecodeString(Extra);

        public List<Definition> Children { get; } = new List<Definition>();


        static HashSet<string> HeadWordsWithNoDefinitions = new HashSet<string>
        {
            "bastal", "booie", "by", "caagh", "co&#8209; prefix.", "cummey", "dyn", "erbee", "fenniu", "fy-",
            "gliee", "giyn",
            "jeear", "jioarey", "keddin", "laee", "lheh", "lhoys", "ner",
            "sheyn", "skew", "sprang-"
        };

        public Definition(string word, string extra)
        {
            this.originalWord = word;
            this.originalExtra = extra;
            Word = DecodeString(word);
            Extra = extra;
        }

        internal static Definition FromHtml(string arg1)
        {
            var split = arg1.Split(",").ToList();


            while (split[0].EndsWith("Times New Roman\"") && split.Count > 1)
            {
                split[0] = split[0] + split[1];
                split.RemoveAt(1);
            }

            // ". See" is a common element which does not contain a comma
            if (split[0].Contains(". See"))
            {
                var original = split[0];
                var index = original.IndexOf(". See");
                split[0] = original.Substring(0, index);
                if (split.Count == 1)
                {
                    split.Add("");
                }

                split[1] = original.Substring(index + ". ".Length) + split[1];
            }

            var word = split.First();

            // Certain headwords (cummey, jeear) appear both with and without definitions
            string definition = null;
            {
                try
                {
                    definition = string.Join(",", split.Skip(1));
                }
                catch
                {
                    if (!HeadWordsWithNoDefinitions.Contains(word.Trim()))
                    {
                        Console.WriteLine($"error: {word}");
                    }
                }
            }

            return new Definition(word, definition);
        }

        internal void AddChild(Definition node)
        {
            this.Children.Add(node);
            node.Parent = this;
        }

        /// <summary>All children including this</summary>
        public IEnumerable<Definition> AllChildren
        {
            get
            {
                yield return this;
                foreach (var child in Children.SelectMany(x => x.AllChildren))
                {
                    yield return child;
                }

            }
        }

        public IEnumerable<String> PossibleWords
        {
            get
            {
                // removing "sic" can cause duplicates
                return getPossibleWords().Select(x => x.Replace('‑', '-').Replace("’", "'").TrimAfter("[sic]", "(sic)", "[sic:", "(sic:").Trim()).Distinct();
            }
        }

        private int? depth = null;
        /// <summary>The nesting depth of an element is defined by the number of spaces before the word in the HTML</summary>
        public int Depth
        {
            get => depth ?? Word.Length - Word.TrimStart().Length;
            set => depth = value;
        }

        public Definition Parent { get; private set; }

        /// <summary>The HTML of the headword(s) for the entry</summary>
        public string Heading => this.originalWord;

        private Dictionary<string, Abbreviation> abbpreviationsAtPrefix = new Dictionary<string, Abbreviation>()
        {
            ["a. d."] = Abbreviation.AdjectiveDerivative,
            ["a. pl."] = Abbreviation.AdjectivePlural,
            ["a."] = Abbreviation.Adjective,
            ["adv. p."] = Abbreviation.AdverbAndPronoun,
            ["adv."] = Abbreviation.Adverb,
            ["comp."] = Abbreviation.ComparativeDegree,
            ["comj."] = Abbreviation.Conjunction,
            ["c. p."] = Abbreviation.ConjunctionAndPronoun,
            ["dim."] = Abbreviation.Diminutive,
            ["em."] = Abbreviation.Emphatically,
            ["f."] = Abbreviation.FeminineGender,
            ["Gal."] = Abbreviation.GalicOrGaelic,
            ["Heb."] = Abbreviation.HebrewAndBookOfHebrews,
            ["id."] = Abbreviation.TheSameAsAbove,
            ["idem."] = Abbreviation.TheSameAsAbove,
            ["in."] = Abbreviation.Interjection,
            ["lit."] = Abbreviation.Literally,
            ["p. p."] = Abbreviation.PrepositionAndPronoun,
            ["p."] = Abbreviation.Pronominal,
            ["pl."] = Abbreviation.HasPlural,
            ["pre."] = Abbreviation.Preposition,
            ["pro."] = Abbreviation.Pronoun,
            ["Prov."] = Abbreviation.ManksProverb,
            ["pt."] = Abbreviation.Participle,
            ["sing."] = Abbreviation.Singular,
            ["s. m. f."] = Abbreviation.DoMasculineAndFeminine, // TODO: Combine
            ["s. m."] = Abbreviation.SubstantiveMasculine,
            ["s. pl."] = Abbreviation.SubstantivePlural,
            ["s. f."] = Abbreviation.SubstantiveFeminine,
            ["s."] = Abbreviation.Substantive,
            ["sup."] = Abbreviation.SuperlativeDegree, 
            ["syn."] = Abbreviation.Synonymous,
            ["v. i."] = Abbreviation.VerbImperative,
            ["v."] = Abbreviation.Verb,
        };

        public List<Abbreviation> Abbreviations
        {
            get
            {
                List<Abbreviation> ret = new List<Abbreviation>();
                foreach (var (k, v) in abbpreviationsAtPrefix)
                {
                    if (Extra.Contains($" {k}") || Extra.StartsWith(k) || Extra.Contains($">{k}"))
                    {
                        ret.Add(v);
                    }
                }

                if (ret.Contains(Abbreviation.Substantive) && 
                    (ret.Contains(Abbreviation.SubstantiveFeminine) || 
                    ret.Contains(Abbreviation.SubstantiveMasculine) || 
                    ret.Contains(Abbreviation.SubstantivePlural)))
                {
                    ret.Remove(Abbreviation.Substantive);
                }

                if (ret.Contains(Abbreviation.Adjective) &&
                    (ret.Contains(Abbreviation.AdjectiveDerivative) ||
                    ret.Contains(Abbreviation.AdjectivePlural)))
                {
                    ret.Remove(Abbreviation.Adjective);
                }

                if (ret.Contains(Abbreviation.Adverb) && ret.Contains(Abbreviation.AdverbAndPronoun))
                {
                    ret.Remove(Abbreviation.Adverb);
                }

                if (ret.Contains(Abbreviation.Article) && ret.Contains(Abbreviation.ArticlePlural))
                {
                    ret.Remove(Abbreviation.Article);
                }

                if (ret.Contains(Abbreviation.Verb) && ret.Contains(Abbreviation.VerbImperative))
                {
                    ret.Remove(Abbreviation.Verb);
                }

                return ret;
            }
        }

        public string EntryText
        {
            get
            {
                var ret = DecodeString(Entry)
                    .TrimAfter("pl. ")
                    .Replace("\r\n", " ")
                    .Replace("\n", " ") // Windows and Linux differ
                    .Replace(" ‑agh;", "")
                    .Replace(" ‑ee;", "")
                    .Replace(" ‑in;", "")
                    .Replace(" ‑ins;", "")
                    .Replace(" ‑ym;", "")
                    .Replace(" ‑yms;", "")
                    .Replace(" ‑ys, 94.", "")
                    ;

                // strip prefixed abreviations
                foreach (var (k,v) in abbpreviationsAtPrefix)
                {
                    var index = ret.IndexOf(k);
                    if (index == -1)
                    {
                        continue;
                    }
                    ret = ret.Substring(index + k.Length);
                }
                return ret.Trim();
            }
        }

        /// <summary>placed before such verbs where two are inserted, as, trog, the verb used alone; the one marked thus, trogg*, is the verb that is to be joined to  agh,  ee,  ey, &c.</summary>
        private static char VERB_HAS_SUFFIXES = '*';

        /// <summary>Reads the word and the definition to obtain the possible words which will link to this definition</summary>
        private IEnumerable<string> getPossibleWords()
        {
           List<string> words = Regex.Split(Word, "\\s+or\\s+").Select(x => x.Trim().Trim(VERB_HAS_SUFFIXES)).ToList();
            foreach (var w in words)
            {
                yield return w;
            }

            // Only apply suffixes to the words matked with an asterisk
            var wordsWithSuffixChanges = Word.Contains(VERB_HAS_SUFFIXES) ? 
                Regex.Split(Word, "\\s+or\\s+")
                .Where(x => x.Contains(VERB_HAS_SUFFIXES))
                .Select(x => x.Trim().Trim(VERB_HAS_SUFFIXES)).ToList() 
                : words;

            // TODO: How to handle "[change -agh to -ee], or  yn."
            var suffixes = Regex.Matches(Entry, "\\s(-|‑)\\w+").Select(x => x.Value.Trim(new char[] { '‑', '-', '\n', '\r', ' ' })).ToList();

            // Handle "[change -agh to -ee]"
            var suffixChanges = Regex.Matches(Entry, "\\[change [-|‑](.*)\\s+to\\s+[-|‑](.*)\\s*]");

            if (suffixChanges.Any())
            {
                foreach (Match changeInSuffix in suffixChanges)
                {
                    var suffixToFind = changeInSuffix.Groups[1].Value.Trim();

                    foreach (var word in wordsWithSuffixChanges)
                    {
                        if (word.EndsWith(suffixToFind))
                        {
                            yield return word.Substring(0, word.Length - suffixToFind.Length) + changeInSuffix.Groups[2].Value;
                        }
                    }

                    suffixes.Remove(suffixToFind);
                    suffixes.Remove(changeInSuffix.Groups[2].Value);
                }
            }

            foreach (var wordAsPrefix in wordsWithSuffixChanges)
            {
                foreach (string suffix in suffixes)
                {
                    yield return wordAsPrefix + suffix;
                }
            }
        }

        private static string DecodeString(string word)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(word);
            var wordAsString = doc.DocumentNode.InnerText;
            return HttpUtility.HtmlDecode(wordAsString).Trim('\r', '\n').Replace("[*]", "*")
                .RemoveBetween("<", ">") // Max uses <> to mark deletions. Since we decoded the InnerText, this is the real character and not the HTML
                .Replace("[cha]", "cha")
                .Replace("[er]", "er")
                .Replace("[ny]", "ny")
                .Replace("[da]", "da")
                .Replace("[lesh]", "lesh")
                .Replace("[yn]", "yn")
                .Replace("[e]", "e")
                .Replace("[ad]", "ad")
                .Replace("[or s'tiark]", "or s'tiark")
                .Replace("[or cruinnaght]", "or cruinnaght")
                ;
        }
    }


    public enum Abbreviation
    {
        Adjective, 
        Adverb, 
        AdjectiveDerivative, 
        AdjectivePlural,
        AdverbAndPronoun, 
        Article, 
        ArticlePlural, 
        ComparativeDegree, 
        Conjunction, 
        ConjunctionAndPronoun, 
        Diminutive, 
        Emphatically, 
        FeminineGender, 
        GalicOrGaelic, 
        HebrewAndBookOfHebrews, 
        TheSameAsAbove, 
        Interjection, 
        Literally, 
        Pronominal, 
        HasPlural, 
        PrepositionAndPronoun, 
        Preposition, 
        Pronoun, 
        ManksProverb, 
        Participle, 
        Substantive, 
        SubstantiveFeminine, 
        Singular, 
        SubstantiveMasculine,
        DoMasculineAndFeminine, 
        SubstantivePlural,
        SuperlativeDegree, 
        Synonymous,
        Verb,
        VerbImperative,
    }

}