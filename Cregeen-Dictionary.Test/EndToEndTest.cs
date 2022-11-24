using Cregeen;
using NUnit.Framework;
using System.Linq;
using static Cregeen_Dictionary.Test.TestUtil.ResourceFetcher;

namespace Cregeen_Dictionary.Test;

[TestFixture]
public class EndToEndTest
{
    [Test]
    public void Aght()
    {
        var html = GetResource("Cregeen_Dictionary.Test.TestData.aght.html");

        var aght = Headword.FromHtmlUnsafe(html);


        // ReSharper disable CommentTypo
        /*
         * aght, s. m. art, skill, behaviour, demeanor, gait, plight, way; pl.  yn. 
            e haght, s. her skill, &c. A 
            gaght, v. act, behave;  agh;  ee;  in;  ins;  ym;  yms;  ys, 94. A 
            gaghtey, v. acting, behaving. A 
            aghtal, a. artful, skilful, dexterous, expert, mannerly. 
            s’aghtal, a. how skilful, artful, &c. A 
            s’aghtaley, a. id., comp. and sup. A
            neu aghtal, a. unskilful, awkward.
            aghtallys, s. m. artfulness, skilfulness. 
            neu-aghtallys, s. f. unskillfulness, &c.
            aght--baghee, s. m. manner of life, occupation; 2 Tim. iii. 10: Agh t’ou uss dy slane er hoiggal my ynsagh, my aght-baghee, my chiarail my chredjue, my hurranse-foddey, my ghraih, my veenid. But thou hast fully known my doctrine, manner of life, purpose, faith, longsuffering, charity, patience; Jonah i. 8: Insh dooin, ta shin guee ort, quoi by-chyndagh ta’n olk shoh er jeet orrin: cre ta dty aght beaghee? Tell us, we pray thee, for whose cause this evil is upon us; What is thine occupation? 
            aghterbee, adv. any way, any wise, any how, however. 
            naght, s. m. the way; with myr [sc. myr naght], like as, that as; a contraction of yn and aght.
            aghtys, s.
            drogh aghtys, s. f. ill behaviour, misdemeanor.
            */
        // ReSharper restore CommentTypo

        Assert.That(aght.Definition.PossibleWords, Is.EquivalentTo(new[] { "aght", "aghtyn" }));
        Assert.That(aght.Definition.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.SubstantiveMasculine, Abbreviation.HasPlural })); // TODO: needs the plural marked
        Assert.That(aght.Definition.EntryText, Is.EqualTo("art, skill, behaviour, demeanor, gait, plight, way"));

        Definition Get(int k) => GetDefinition(aght, k);

        var eHaght = Get(0);
        Assert.That(eHaght.PossibleWords, Is.EquivalentTo(new[] { "e haght", "haght" }));
        Assert.That(eHaght.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.Substantive }));
        Assert.That(eHaght.EntryText, Is.EqualTo("her skill, &c."));
        Assert.That(eHaght.DerivedFromLetter?.Marking, Is.EqualTo("A"));

        var gaght = Get(1);
        Assert.That(gaght.PossibleWords, Is.EquivalentTo(new[] { "gaght", "gaghtagh", "gaghtee", "gaghtin", "gaghtins", "gaghtym", "gaghtyms", "gaghtys" }));
        Assert.That(gaght.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.Verb }));
        Assert.That(gaght.EntryText, Is.EqualTo("act, behave"));
        Assert.That(gaght.DerivedFromLetter?.Marking, Is.EqualTo("A"));

        var gaghtey = Get(2);
        Assert.That(gaghtey.PossibleWords, Is.EquivalentTo(new[] { "gaghtey" }));
        Assert.That(gaghtey.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.Verb }));
        Assert.That(gaghtey.EntryText, Is.EqualTo("acting, behaving."));
        Assert.That(gaghtey.DerivedFromLetter?.Marking, Is.EqualTo("A"));

        var aghtal = Get(3);
        Assert.That(aghtal.PossibleWords, Is.EquivalentTo(new[] { "aghtal" }));
        Assert.That(aghtal.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.Adjective }));
        Assert.That(aghtal.EntryText, Is.EqualTo("artful, skilful, dexterous, expert, mannerly."));

        Assert.Inconclusive();
    }

    
    [Test]
    public void Baccagh()
    {
        var html = GetTestData("baccagh");
        
        // ReSharper disable CommentTypo
        /*
baccagh, a. halt, maimed. 
s’baccagh, a. how halt or maimed B 
s’baccee, a. id., comp. and sup. B
baccagh, s. m. a person halt or disabled; pl. 71 [change  agh to  ee]. 
yn vaccagh, s. the halt person. B
nyn maccagh, s. your, &c. halt, &c. person. B

         */
        // ReSharper enable CommentTypo
        var baccagh = Headword.FromHtmlUnsafe(html);
        
        Assert.That(baccagh.Definition.PossibleWords, Is.EquivalentTo(new[] { "baccagh" }));
        Assert.That(baccagh.Definition.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.Adjective }));
        Assert.That(baccagh.Definition.EntryText, Is.EqualTo("halt, maimed."));

        Definition Get(int k) => GetDefinition(baccagh, k);

        var sbaccagh = Get(0);
        Assert.That(sbaccagh.EntryText, Is.EqualTo("how halt or maimed"));
        var sBaccee = Get(1);
        Assert.That(sBaccee.EntryText, Is.EqualTo("how halt or maimed")); // id. means identical to the previous
        var baccaghSm = Get(2);
        Assert.That(baccaghSm.EntryText, Is.Not.Empty);
        var ynVaccagh = Get(3);
        Assert.That(ynVaccagh.EntryText, Is.Not.Empty);
        var nynMaccagh = Get(4);
        Assert.That(nynMaccagh.EntryText, Is.Not.Empty);
        
        Assert.Inconclusive();
    }

    [Test]
    public void GreinAadjyn()
    {
        var html = GetTestData("grein-aadjyn");
        var greinAadjyn = Headword.FromHtmlUnsafe(html);

        var definition = greinAadjyn.Definition;
        
        Assert.That(definition.PossibleWords, Is.EquivalentTo(new[] { "grein-aadjyn"}));
        Assert.That(definition.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.SubstantivePlural, Abbreviation.HasPlural }));
        Assert.That(definition.EntryText, Is.EqualTo("greaves [OED: Branches, twigs]."));
    }
    
    [Test]
    public void DaHtml()
    {
        var html = GetTestData("da");
        var da = Headword.FromHtmlUnsafe(html);
        Assert.That(da.Definition.Word, Is.EqualTo("da"));
        Assert.That(da.Definition.EntryText, Does.StartWith("to him, for him, him, to, for; as, chur mee da eh (I gave it to him); te aym da (I have it for him); lhig da (let him); eeck da Cesar (pay to Cesar); ‑syn, id. em. "));
    }

    [Test]
    public void SoylTest()
    {
        // improves suffix stripping
        var html = GetTestData("soyl");
        var soyl = Headword.FromHtmlUnsafe(html);
        
        Assert.That(soyl.Definition.EntryText, Is.EqualTo("compare, typify"));
    }
    
    [Test]
    public void Thannys()
    {
        var html = GetTestData("thannys-snippet");
        var thannys = Headword.FromHtmlUnsafe(html).Definition;
        
        Assert.That(thannys.Word, Is.Not.EqualTo("thannys")); // sub-word
        Assert.That(thannys.EntryText, Is.EqualTo("thin, rarify"));
    }

    [Test]
    public void Aaitn()
    {
        var html = GetTestData("aaitn-snippet");
        var aaitn = Headword.FromHtmlUnsafe(html).Definition;
        Assert.That(aaitn.Word, Is.EqualTo("aaitn"));
        Assert.That(aaitn.EntryText, Is.EqualTo("gorse, cover with whins"));
    }
    
    
    [Test]
    public void Dobberan()
    {
        // we had an issue with a bad replacement
        var html = GetTestData("dobberan");
        var dobberan = Headword.FromHtmlUnsafe(html).Definition;
        Assert.That(dobberan.Word, Is.EqualTo("dobberan"));
        Assert.That(dobberan.EntryText, Does.Not.StartWith("d"));
    }
    
    private static Definition GetDefinition(Headword word, int k) => word.Definition.Children.Skip(k).First();
}