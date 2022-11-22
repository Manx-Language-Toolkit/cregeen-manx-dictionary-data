using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static Cregeen.Abbreviations;

namespace Cregeen;

[DebuggerDisplay("{_originalWord}")]
public class Definition
{
    /// <summary>
    /// Html word
    /// </summary>
    private string _originalWord;
    /// <summary>
    /// Html Extra
    /// </summary>
    // ReSharper disable once NotAccessedField.Local
    private readonly string _originalExtra;


    /// <summary>
    /// Decoded word
    /// </summary>
    public string Word { get; }
    public string Extra { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    public string Entry { get; }

    public List<Definition> Children { get; } = new();


    static readonly HashSet<string> HeadWordsWithNoDefinitions = new()
    {
        "bastal", "booie", "by", "caagh", "co&#8209; prefix.", "cummey", "dyn", "erbee", "fenniu", "fy-",
        "gliee", "giyn",
        "jeear", "jioarey", "keddin", "laee", "lheh", "lhoys", "ner",
        "sheyn", "skew", "sprang-"
    };

    private Definition(string word, string extra)
    {
        _originalWord = word;
        _originalExtra = extra;
        Word = DecodeString(word);
            
            
        Extra = extra;
        var potentialEntry = DecodeString(Extra);
        var (derivativeMarking, entry2) = DerivativeMarking.ParseDerivativeMarking(potentialEntry);
        DerivedFromLetter = derivativeMarking;
        Entry = entry2;
    }

    /// <summary>Defines the letter this derivative/aspiration should be listed under</summary>
    public DerivativeMarking? DerivedFromLetter { get;  }

    [Pure]
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
            var index = original.IndexOf(". See", StringComparison.Ordinal);
            split[0] = original.Substring(0, index);
            if (split.Count == 1)
            {
                split.Add("");
            }

            split[1] = original.Substring(index + ". ".Length) + split[1];
        }

        var word = split.First();

        // Certain headwords (cummey, jeear) appear both with and without definitions
        string definition = "";
        {
            try
            {
                definition = string.Join(",", split.Skip(1));
            }
            catch
            {
                if (!HeadWordsWithNoDefinitions.Contains(word.Trim()))
                {
                    throw new NotImplementedException();
                }
            }
        }

        return new Definition(word, definition);
    }

    internal void AddChild(Definition node)
    {
        Children.Add(node);
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
            return GetPossibleWords()
                .Select(x => x.Replace('‑', '-')
                    .Replace("’", "'")
                    .TrimAfter("[sic]", "(sic)", "[sic:", "(sic:")
                    .Trim())
                .Distinct();
        }
    }

    private int? _depth;
    /// <summary>The nesting depth of an element is defined by the number of spaces before the word in the HTML</summary>
    public int Depth
    {
        get => _depth ?? Word.Length - Word.TrimStart().Length;
        set => _depth = value;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Definition? Parent { get; private set; }

    /// <summary>The HTML of the headword(s) for the entry</summary>
    public string Heading => _originalWord;



    public List<Abbreviation> Abbreviations => ParseAbbreviations(Extra);

    /// <summary>
    /// Override for Entry text if <see cref="Abbreviation.TheSameAsAbove"/> is set.
    /// </summary>
    public string? EntryTextOverride { get; set; }
    
    /// <summary>
    /// The definition of the word, exclusive of notes such as Examples or Bible verses.
    /// </summary>
    public string EntryText
    {
        get
        {
            if (EntryTextOverride != null)
            {
                return EntryTextOverride;
            }
            var ret = DecodeString(Entry)
                    .TrimAfter("pl. -") // pl. -yn, pl. -ghyn. But don't trim after something like:
                    .TrimAfter("pl. ‑") // and with the non-breaking hypen
                    // grein-aadjyn, s. pl. <Definition>
                    .Replace("\r\n", " ")
                    .Replace("\n", " ") // Windows and Linux differ
                    .Replace(" ‑agh;", "")
                    .Replace(" ‑ee;", "")
                    .Replace(" ‑in;", "")
                    .Replace(" ‑ins;", "")
                    .Replace(" ‑ym;", "")
                    .Replace(" ‑yms;", "")
                    .Replace(" ‑ys, 94.", "")
                    .Trim()
                ;

            var ret1 = StripPrefixes(ret);
            var ret2 = StripSuffixes(ret1);
            return ret2.Trim();
        }
    }

    private static string StripSuffixes(string ret)
    {
        
        var suffixes = SuffixUtils.Suffixes
            .Concat(new[]
            {
                ";",
                ",",
            }).ToList();
        
        // trim a full stop, only if we have a match as well
        if (suffixes.Any(x => ret.EndsWith(x + ".")))
        {
            ret = ret[..^1]; 
        }
        
        while (suffixes.Any(x => ret.EndsWith(x)))
        {
            var suffix = suffixes.First(suffix => ret.EndsWith(suffix));
            ret = TrimAfter(ret, suffix).Trim();
        }

        return ret;
    }

    private static string StripPrefixes(string ret)
    {
        // strip prefixed abbreviations (and anything before them)
        var prefixes = PrefixToAbbreviation.Keys
            .Concat(new[]
            {
                "8.",
                ",",
                "85. ",
            })
            .OrderByDescending(x => x.Length).ToList();
        while (prefixes.Any(x => ret.StartsWith(x)))
        {
            var prefix = prefixes.First(prefix => ret.StartsWith(prefix));
            ret = TrimBefore(ret, prefix).Trim();
        }

        // except plural - we only want to strip that if it comes first. 
        // Example:  <definition> pl. see enmyn.
        if (ret.Trim().StartsWith("pl."))
        {
            ret = TrimBefore(ret, "pl.");
        }

        return ret;
    }

    private static string TrimBefore(string input, string prefix)
    {
        var index = input.IndexOf(prefix, StringComparison.Ordinal);
        if (index == -1)
        {
            return input;
        }
        return input.Substring(index + prefix.Length);
    }
    
    private static string TrimAfter(string input, string prefix)
    {
        var index = input.LastIndexOf(prefix, StringComparison.Ordinal);
        if (index == -1)
        {
            return input;
        }
        return input[..index];
    }

    /// <summary>placed before such verbs where two are inserted, as, trog, the verb used alone; the one marked thus, trogg*, is the verb that is to be joined to  agh,  ee,  ey, &amp;c.</summary>
    private const char VerbHasSuffixes = '*';

    /// <summary>Reads the word and the definition to obtain the possible words which will link to this definition</summary>
    private IEnumerable<string> GetPossibleWords()
    {
        List<string> words = Regex.Split(Word, "\\s+or\\s+").Select(x => x.Trim().Trim(VerbHasSuffixes)).ToList();
        foreach (var w in words)
        {
            yield return w;
        }

        // handle 'bold' words: dy <b>hroggal</b> should return "dy hroggal" and "hroggal"
        if (Heading.Contains("<b>"))
        {
            // Sometimes we have <b>gheul</b>* &lt;or <b>gheuley</b>&gt;
            // Max Indicates additions with < > so we do not include these for now.
            var withBracesTrimmed = Utils.TrimBraces(Heading);
            var boldHeadings = Utils.GetBoldWords(withBracesTrimmed);
            foreach (var heading in boldHeadings)
            {
                var value = HttpUtility.HtmlDecode(heading); // convert &#8209; to -
                if (!value.Contains('\r') && !value.Contains('\n') && !value.Contains("[") && !value.Contains("*") && !String.IsNullOrWhiteSpace(value))
                {
                    yield return value;    
                }
            }
        }
            
        // Only apply suffixes to the words matked with an asterisk
        var wordsWithSuffixChanges = Word.Contains(VerbHasSuffixes) ? 
            Regex.Split(Word, "\\s+or\\s+")
                .Where(x => x.Contains(VerbHasSuffixes))
                .Select(x => x.Trim().Trim(VerbHasSuffixes)).ToList() 
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
                .Replace("[m]", "m") // chaboon
                .Replace("[or s'tiark]", "or s'tiark")
                .Replace("[or cruinnaght]", "or cruinnaght")
            ;
    }
}