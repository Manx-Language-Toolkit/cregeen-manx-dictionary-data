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
}