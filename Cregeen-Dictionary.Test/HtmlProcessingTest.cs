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
}