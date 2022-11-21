using System;
using Cregeen;
using NUnit.Framework;

namespace Cregeen_Dictionary.Test;

public class UtilsTest
{
    [Test]
    public void GetBoldWords()
    {
        var text = "<i>\n        dy </i><b>hroggal</b>";
        var words = Utils.GetBoldWords(text);
        Assert.That(words, Has.Count.EqualTo(1));
        Assert.That(words, Does.Contain("hroggal"));
    }
    
    [Test]
    public void TrimBraces()
    {
        var text = "<b>gheul</b>* &lt;or <b>gheuley</b>&gt;";
        var trimmed = Utils.TrimBraces(text);
        Assert.That(trimmed, Is.EqualTo("<b>gheul</b>* "));
    }
}