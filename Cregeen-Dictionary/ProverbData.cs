using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Cregeen;

[DebuggerDisplay("{Proverb}")]
public class ProverbData
{
    public string Proverb { get; set; }
    
    [Pure]
    public static (ProverbData?, string) PraseProverb(string param)
    {
        var index = param.IndexOf("Prov.", StringComparison.Ordinal);
        if (index == -1)
        {
            return (null, param);
        }

        var proverb = new ProverbData { Proverb = param[("Prov.".Length + index)..].Trim() };
        return (proverb, param[..index]);
    }
}