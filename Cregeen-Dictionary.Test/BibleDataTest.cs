using Cregeen;
using NUnit.Framework;

namespace Cregeen_Dictionary.Test;

public class BibleDataTest
{
    [Test]
    public void TestAll()
    {
        {
            string goodInput = "bishoprick; Acts, i. 20: Son te";
            var (data, remaining) = BibleData.PraseBibleBook(goodInput);
            Assert.That(data, Is.Not.Null);
            Assert.That(data?.BibleExample, Is.EqualTo(", i. 20: Son te"));
            Assert.That(remaining, Is.EqualTo("bishoprick; "));
        }
    }

    [Test]
    public void RealWorldTest()
    {
        // ReSharper disable line StringLiteralTypo
        string input = @" s. m. bishoprick; Acts, i. 20: Son te scruit ayns
lioar ny psalmyn, Lhig da’n ynnyd-vaghee echey ve follym faase, as ny lhig da
dooinney erbee cummal ayn: as, Yn aspickys echey
lhig da fer elley y ghoaill. For it is written in the book of Psalms, Let
his habitation be desolate, and let no man dwell therein: and his bishoprick
let another take. ";
        
        var (data, remaining) = BibleData.PraseBibleBook(input);
        Assert.That(data, Is.Not.Null);
        Assert.That(data?.BibleExample, Does.Contain("i. 20"));
        Assert.That(remaining, Is.EqualTo(" s. m. bishoprick; "));
    }
}