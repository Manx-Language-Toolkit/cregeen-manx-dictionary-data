using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Cregeen;

/// <summary>
/// A, B, C, Ch, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, Sh, Sl, T, U, V, W, or Y,
/// at the end of a line, shows that the word is a derivative or aspiration of one whose initial radically is A or B, etc.
/// C, placed after ch, shows it to be an aspiration of a word radically without an h,
/// and so for G placed after gh, P after ph, etc.
/// </summary>
[DebuggerDisplay("{Marking}")]
public class DerivativeMarking
{
    public string Marking { get; set; }
    
    [Pure]
    public static (DerivativeMarking?, string) ParseDerivativeMarking(string param)
    {
        var input = param.Trim().TrimEnd('.');

        var allEndings = ValidEndings.SelectMany(x => new[] { x, $"[{x}]" });
        var detectedEnding = allEndings.SingleOrDefault(ending => input.EndsWith(" " + ending) || input.EndsWith(" " + ending + "."));
        if (detectedEnding == null)
        {
            return (null, param);
        }

        var marking = new DerivativeMarking { Marking = detectedEnding.Trim('[', ']') };
        

        return (marking, input[..(input.Length - detectedEnding.Length - " ".Length)]);
    }

    private static readonly string[] ValidEndings = {
        "A", "B", "C", "Ch", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "Sh", "Sl", "T", "U", "V", "W", "Y"
    };
}