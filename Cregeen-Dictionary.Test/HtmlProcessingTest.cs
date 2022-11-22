using System.Linq;
using Cregeen;
using NUnit.Framework;

namespace Cregeen_Dictionary.Test;

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
}