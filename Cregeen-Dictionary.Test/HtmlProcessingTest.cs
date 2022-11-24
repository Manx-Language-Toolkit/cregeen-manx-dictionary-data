using System.Linq;
using Cregeen;
using NUnit.Framework;

namespace Cregeen_Dictionary.Test;

// TODO: cha greck DerivativeMarking is incorrect
// TODO: dynsee DerivativeMarking is incorrect
// TODO: geinnagh - check suffix
// TODO: n'eeastaghey
public class HtmlProcessingTest
{
    [Test]
    public void HroggalHtml()
    {
        var html = "<i>\n        dy </i><b>hroggal</b>,<i> v. </i>to lift, rear, build, train, &amp;c. T<i>";
        var definition = Headword.FromHtmlUnsafe(html);
        Assert.That(definition.Definition.AllPossibleWords(), Does.Contain("hroggal"));
    }
    
    [Test]
    public void EnnymHtml()
    {
        var ennym = Headword.FromHtmlUnsafe("<b>ennym</b>,<i> s. m.</i> name, epithet, appellation;<i> pl. </i>see <i>enmyn.").Definition;
        Assert.That(ennym.Word, Is.EqualTo("ennym"));
        Assert.That(ennym.Extra, Is.EqualTo("<i> s. m.</i> name, epithet, appellation;<i> pl. </i>see <i>enmyn."));
        Assert.That(ennym.EntryText, Is.EqualTo("name, epithet, appellation; pl. see enmyn."));
    }

    [Test]
    public void Lioroo()
    {
        var headword = Headword.FromHtmlUnsafe(@"
</i><b><u>lio</u>ree<span style='color:red'>-</span>hene</b>, <i>p.&nbsp;p. </i>by
herself. <br>
<b>lioroo</b>, <i>p.&nbsp;p. </i>by them; <b>&#8209;syn</b>,<i> id. em.");


        Assert.That(headword.Definition.EntryText, Is.EqualTo("by herself."));
        
        var lioroo = headword.All.Skip(1).Single();
        Assert.That(lioroo.Word, Is.EqualTo("lioroo"));
        Assert.That(lioroo.PossibleWords, Does.Contain("lioroosyn"));
        Assert.That(lioroo.Abbreviations, Does.Contain(Abbreviation.Emphatically));
        Assert.That(lioroo.Abbreviations, Does.Contain(Abbreviation.TheSameAsAbove));
        Assert.That(lioroo.EntryText, Is.EqualTo("by them; by herself."));
    }

    [Test]
    public void Anchredjuee()
    {
        // ReSharper disable line StringLiteralTypo
        var headword = Headword.FromHtmlUnsafe("<b>an<u>chred</u>juagh</b>,<i> s. m.</i> an unbeliever; <i>pl. </i>71 [change &#8209;<b>agh</b> to &#8209;<b>ee</b>].<i> <br>").Definition;
        Assert.That(headword.Word, Is.EqualTo("anchredjuagh"));
        Assert.That(headword.PossibleWords, Does.Contain("anchredjuee"));
        Assert.That(headword.EntryText, Is.EqualTo("an unbeliever"));
    }

    [Test]
    public void SEulyssee()
    {
        var headword = Headword.FromHtmlUnsafe(@"</i><b>s’eulyssagh</b>,<i> a. </i>how indignant or inflamed with anger,
furious. E<br>
<b>s’eulyssee</b>,<i> a. id.,</i> [<i>comp.</i> and <i>sup.</i>,]<i> </i>58. E</p>");
        Assert.That(headword.Definition.Word, Is.EqualTo("s’eulyssagh"));

        var sEulyssee = headword.All.Skip(1).First();
        Assert.That(sEulyssee.EntryText, Is.EqualTo("how indignant or inflamed with anger, furious."));
    }

    [Test]
    public void Dwhaayl()
    {
        var headword =
            Headword.FromHtmlUnsafe(@"<b>dwhaayl</b>, <i>v.</i> did sew, sewed; &#8209;<b>agh</b>; &#8209;<b>in</b>;
-<b>ins</b>; &#8209;<b>ym</b>; &#8209;<b>yms</b>, <a href=""#R94"">94</a>. W.<br>").Definition;
        
        Assert.That(headword.EntryText, Is.EqualTo("did sew, sewed"));
    }
    
    
    
    [Test]
    public void Dynsee()
    {
        var dynsee =
            Headword.FromHtmlUnsafe(@"<b>dynsee</b>, <i>v. </i>taught, did teach, learned or did learn, did gain, or
impart knowledge. Y. <br>").Definition;
        
        Assert.That(dynsee.DerivedFromLetter?.Marking, Is.EqualTo("Y"));
        Assert.That(dynsee.EntryText, Is.EqualTo("taught, did teach, learned or did learn, did gain, or impart knowledge."));
    }

    [Test]
    public void SJeebinagh()
    {
        var hw = Headword.FromHtmlUnsafe(@"<b>s’jeebinagh</b>,<i> a. </i>how full of net work. J<br>
<b>s’jeebinee</b>,<i> a. id.</i>, [<i>comp.</i> and <i>sup.</i>,] 58, J<i><br>");
        
        
        Assert.That(hw.Definition.EntryText, Is.EqualTo("how full of net work."));
        Assert.That(hw.All.Count(), Is.EqualTo(2));
        Assume.That(hw.All.Skip(1).Single().EntryText, Is.EqualTo("how full of net work."));
    }
}