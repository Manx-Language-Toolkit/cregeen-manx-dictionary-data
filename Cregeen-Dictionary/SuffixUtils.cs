using System.Collections.Generic;

namespace Cregeen;

public static class SuffixUtils
{
    /// <summary>
    /// A list of suffixes which we can strip
    /// </summary>
    public static readonly List<string> Suffixes = new()
    {
        "‑agh, 77", 
        "‑ee, 80", 
        "‑in, 83", 
        "‑ins, 84", 
        "‑ym, 86", 
        "‑yms, 87", 
        "‑ys, 88"
    };
}