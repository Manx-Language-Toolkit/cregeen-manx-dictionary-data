using System.Collections.Generic;
using static Cregeen.Abbreviation;
using static Cregeen.AbbreviationExtensions.Gender;
using static Cregeen.AbbreviationExtensions.PartOfSpeech;

namespace Cregeen;

public static class Abbreviations
{
    public static readonly Dictionary<string, Abbreviation> PrefixToAbbreviation = new()
    {
        ["a. d."] = AdjectiveDerivative,
        ["a. pl."] = AdjectivePlural,
        ["a."] = Abbreviation.Adjective,
        ["adv. p."] = AdverbAndPronoun,
        ["adv."] = Abbreviation.Adverb,
        ["comp."] = ComparativeDegree,
        ["comj."] = Abbreviation.Conjunction,
        ["c. p."] = ConjunctionAndPronoun,
        ["dim."] = Diminutive,
        ["em."] = Emphatically,
        ["f."] = FeminineGender,
        ["Gal."] = GalicOrGaelic,
        ["Heb."] = HebrewAndBookOfHebrews,
        ["id."] = TheSameAsAbove,
        ["idem."] = TheSameAsAbove,
        ["in."] = Abbreviation.Interjection,
        ["lit."] = Literally,
        ["p. p."] = PrepositionAndPronoun,
        ["p."] = Pronominal,
        ["pl."] = HasPlural,
        ["pre."] = Abbreviation.Preposition,
        ["pro."] = Abbreviation.Pronoun,
        ["Prov."] = ManksProverb,
        ["pt."] = Participle,
        ["sing."] = Singular,
        ["s. m. f."] = DoMasculineAndFeminine, // TODO: Combine
        ["s. m."] = SubstantiveMasculine,
        ["s. pl."] = SubstantivePlural,
        ["s. f."] = SubstantiveFeminine,
        ["s."] = Substantive,
        ["sup."] = SuperlativeDegree,
        // TODO: feeaghyn
        ["syn."] = Synonymous,
        ["v. i."] = VerbImperative,
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

        if (ret.Contains(Substantive) && 
            (ret.Contains(SubstantiveFeminine) || 
             ret.Contains(SubstantiveMasculine) || 
             ret.Contains(SubstantivePlural)))
        {
            ret.Remove(Substantive);
        }

        if (ret.Contains(Abbreviation.Adjective) &&
            (ret.Contains(AdjectiveDerivative) ||
             ret.Contains(AdjectivePlural)))
        {
            ret.Remove(Abbreviation.Adjective);
        }

        if (ret.Contains(Abbreviation.Adverb) && ret.Contains(AdverbAndPronoun))
        {
            ret.Remove(Abbreviation.Adverb);
        }

        if (ret.Contains(Abbreviation.Article) && ret.Contains(ArticlePlural))
        {
            ret.Remove(Abbreviation.Article);
        }

        if (ret.Contains(Abbreviation.Verb) && ret.Contains(VerbImperative))
        {
            ret.Remove(Abbreviation.Verb);
        }

        return ret;
    }
}


public enum Abbreviation
{
    // There is no 'm.' listed in the abbreviations for masculine
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

public static class AbbreviationExtensions
{
    
    // noun, pronoun, verb, adjective, adverb, preposition, conjunction, interjection, article
    private static readonly Dictionary<Abbreviation, IList<PartOfSpeech>> PartsOfSpeech = new()
    {
        // nouns
        [Substantive] = new[] {Noun},
        [SubstantiveFeminine] = new[] {Noun},
        [SubstantiveMasculine] = new[] {Noun},
        [DoMasculineAndFeminine] = new[] {Noun}, 
        [SubstantivePlural] = new[] {Noun},
        // pronouns
        [Abbreviation.Pronoun] = new []{ PartOfSpeech.Pronoun}, 
        [Pronominal] = new []{PartOfSpeech.Pronoun}, 
        [ConjunctionAndPronoun] = new []{PartOfSpeech.Pronoun, PartOfSpeech.Conjunction}, 
        [AdverbAndPronoun] = new []{PartOfSpeech.Pronoun, PartOfSpeech.Adverb}, 
        [PrepositionAndPronoun] = new []{PartOfSpeech.Pronoun, PartOfSpeech.Preposition}, 
        // verbs
        [Abbreviation.Verb] = new []{PartOfSpeech.Verb}, 
        [VerbImperative] = new []{PartOfSpeech.Verb},  // TODO
        // adjective
        [Abbreviation.Adjective] = new[] {PartOfSpeech.Adjective},
        [AdjectiveDerivative] = new[] {PartOfSpeech.Adjective},
        [AdjectivePlural] = new[] {PartOfSpeech.Adjective},
        // adverb
        [Abbreviation.Adverb] = new[] {PartOfSpeech.Adverb}, 
        // [AdverbAndPronoun] = new[] {PartOfSpeech.Adverb}, 
        // preposition
        [Abbreviation.Preposition] = new[] {PartOfSpeech.Preposition}, 
        // [PrepositionAndPronoun] = new[] {PartOfSpeech.Preposition}, 
        // conjunction
        [Abbreviation.Conjunction] = new[] {PartOfSpeech.Conjunction}, 
        // [ConjunctionAndPronoun] = new[] {PartOfSpeech.Conjunction}, 
        // interjection
        [Abbreviation.Interjection] = new[] {PartOfSpeech.Interjection}, 
        // article
        [Abbreviation.Article] = new[] {PartOfSpeech.Article},  
        [ArticlePlural] = new[] {PartOfSpeech.Article}, 
        // additional
        // ComparativeDegree, SuperlativeDegree, 
        // Diminutive
    };
    public static IList<PartOfSpeech> GetPartsOfSpeech(this Abbreviation abbreviation) => PartsOfSpeech.GetValueOrDefault(abbreviation, new List<PartOfSpeech>());
    
    public enum PartOfSpeech
    {
        Noun, Pronoun, Verb, Adjective, Adverb, Preposition, Conjunction, Interjection, Article
    }

    private static readonly Dictionary<Abbreviation, IList<Gender>> Genders = new()
    {
        [SubstantiveFeminine] = new[] { Feminine },
        [SubstantiveMasculine] = new[] { Masculine },
        [DoMasculineAndFeminine] = new[] { Masculine, Feminine },
        [FeminineGender] = new[] { Feminine },
    };
    
    public static IList<Gender> GetGender(this Abbreviation abbreviation) =>
        Genders.GetValueOrDefault(abbreviation, new List<Gender>());

    public enum Gender
    {
        Masculine, Feminine
    }
}
