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
        "‑ail, 78",
        "‑al, 79",
        "‑ee, 80", 
        "‑eil, 81",
        "‑ey, 82",
        "‑in, 83", 
        "‑ins, 84", 
        "‑it, 85",
        "‑ym, 86", 
        "‑yms, 87", 
        "‑ys, 88",
        "‑ys. 88",
        " 89",
        " 85",
        " em.",
        " id.",
        "‑syn",
        "‑in",
        "‑ish",
        "‑yn",
        " 58",
        "[comp. and sup.,]",
        " [F]",
        " id. em", // bailleuish
        " pl. 71 [change ‑agh to ‑ee].",
        " pl. 67 [change ‑ey to ‑aghyn]",
    };
}