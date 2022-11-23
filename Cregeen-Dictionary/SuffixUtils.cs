using System.Collections.Generic;

namespace Cregeen;

public static class SuffixUtils
{
    /// <summary>
    /// A list of suffixes which we can strip
    /// </summary>
    public static readonly List<string> Suffixes = new()
    {
        " ‑agh,77",
        "‑agh, 77", 
        "‑ail, 78",
        "‑al, 79",
        "‑ee, 80", 
        "‑ee 80",
        "‑eil, 81",
        "‑aghey, 82", 
        "‑ey, 82",
        "‑yn, 83",
        "‑in, 83", 
        "‑ins, 84", 
        "‑ins. 84",
        "‑it, 85",
        "‑ym, 86",
        "‑yn, 86",
        "‑yms. 87",
        "‑yms, 87", 
        "‑ys, 88",
        "‑in, 88",
        "‑ys. 88",
        "‑ys, 94",
        "‑yms, 94",
        "‑yms, 94.",
        "‑ym, 94.",
        " 89",
        " 85",
        " em.",
        " id.",
        "‑agh.",
        "‑s",
        "‑syn",
        "‑al",
        "‑in",
        "‑ghyn.", // TODO: confirm
        "‑‑agh",
        "‑agh",
        "‑ey",
        "‑it",
        "‑ish",
        "-ins",
        "‑yn",
        "‑ys",
        "‑ys.",
        "‑ym",
        "‑yms",
        "‑aghey",
        " 58",
        " 87",
        "[comp. and sup.,]",
        " comp. and sup.",
        " [F]",
        " A",
        " B",
        " C",
        " Ch",
        " D",
        " E",
        " F",
        " J",
        " I",
        " M",
        " [M]",
        "77; &c.",
        "‑ym, &c",
        "G .", // see if this needs fixing
        " see 62.",
        " W.",
        " id. em", // bailleuish
        " pl. 71 [change ‑agh to ‑ee]",
        " pl. 72 [change ‑agh to ‑eeyn]",
        " pl. 67 [change ‑ey to ‑aghyn]",
        " pl. 69 [change ‑ey to ‑yn]", // TODO: confirm
        " pl. 71 [change ‑agh to ‑ee]", // TODO: confirm
        " pl. 71 [change ‑iagh to ‑ee]", // TODO: confirm
        " pl. 71.", // TODO: check
        " pl. 67.", // TODO: check
        " pl. 76.", // TODO: check
    };
}