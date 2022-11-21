using System.Collections.Generic;

namespace Cregeen;

public static class Abbreviations
{
    public static readonly Dictionary<string, Abbreviation> PrefixToAbbreviation = new()
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

    public static List<Abbreviation> ParseAbbreviations(string toParse)
    {
        var ret = new List<Abbreviation>();
        foreach (var (k, v) in PrefixToAbbreviation)
        {
            if (toParse.Contains($" {k}") || toParse.StartsWith(k) || toParse.Contains($">{k}"))
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