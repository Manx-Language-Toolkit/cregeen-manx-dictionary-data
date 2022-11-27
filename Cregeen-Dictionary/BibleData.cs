using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Cregeen;

[DebuggerDisplay("{BibleExample}")]
public class BibleData
{
    public string BibleExample { get; set; }
    // TODO: We should store the book here as well
    
    [Pure]
    public static (BibleData?, string) PraseBibleBook(string param)
    {
        // PERF: filter bookNames first
        var bookNames = books.Concat(books.Select(x => x + ","))
            .Select(x => x + " ") // both have a space after
            .SelectMany(x => new[] { x + "i", x + "v", x + "x", x + "l" }) // quick check for roman numerals next
            .SelectMany(x => new[] { x, "1 " + x, "2 " + x });

        var validIndices = bookNames.Select(x => (x, index: param.IndexOf(x, StringComparison.Ordinal))).Where(x => x.index != -1).ToList();
        if (!validIndices.Any())
        {
            return (null, param);
        }

        var (book, index) = validIndices.MinBy(x => x.index);
        

        var example = new BibleData { BibleExample = param[(book.Length + index - " i".Length)..].Trim() };
        return (example, param[..index]);
    }

    private static string[] books = { 
        "Mat.", "Matt.",
        "Mark", 
        "Luke", 
        "John",
        "Acts",
        "Rom.",
        "Cor.",
        
        "Amos",
        "Micah",
        "Jas",
        "Hos.",
        "Cant.",
        "Zec.",
        "Chron.", 
        "Sam.", 
        "Dan.", "Job", 
        "Psalms", "Psal.", "Psl.", "Psl.", 
        "Luke",
        "Dev.",
        "Ez.",
        "Gal.",
        "Josh.",
        "Deut.",
        "[Jer.]",
        "Joel",
        "Esther",
        "Jer.",
        "Lev.",
        "Ecclesiasticus",
        "Pro.",
        "Heb.",
        "Num.", "Numbers",
        "Rev",
        "Zep.",
        "Ezra",
        "Deu.",
        "[Wisdom of Solomon]",
        "Kings", "Gen.", "Jud.",  "Exod.", "Ex.", 
        "Isa.", "Isaiah" };
}