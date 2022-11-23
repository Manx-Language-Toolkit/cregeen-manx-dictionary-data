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
        "‑ee 80",
        "‑eil, 81",
        "‑aghey, 82", 
        "‑ey, 82",
        "‑in, 83", 
        "‑ins, 84", 
        "‑it, 85",
        "‑ym, 86",
        "‑yms, 87", 
        "‑ys, 88",
        "‑in, 88",
        "‑ys. 88",
        "‑ys, 94",
        "‑yms, 94",
        "‑yms, 94.",
        " 89",
        " 85",
        " em.",
        " id.",
        "‑syn",
        "‑al",
        "‑in",
        "‑it",
        "‑ish",
        "-ins",
        "‑yn",
        " 58",
        "[comp. and sup.,]",
        " comp. and sup.",
        " [F]",
        " C",
        " W.",
        " id. em", // bailleuish
        " pl. 71 [change ‑agh to ‑ee].",
        " pl. 67 [change ‑ey to ‑aghyn]",
    };
}