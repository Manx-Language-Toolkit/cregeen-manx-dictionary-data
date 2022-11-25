using NUnit.Framework;
using static Cregeen.ProverbData;

namespace Cregeen_Dictionary.Test;

public class ProverbDataTest
{
    [Test]
    public void All()
    {
        {
            var s = "Prov. AAA";
            var (proverb, rest) = PraseProverb(s);
            Assert.That(proverb?.Proverb, Is.EqualTo("AAA"));
            Assert.That(rest, Is.EqualTo(""));
        }
        {
            var s = "AAA";
            var (proverb, rest) = PraseProverb(s);
            Assert.That(proverb?.Proverb, Is.Null);
            Assert.That(rest, Is.EqualTo("AAA"));
        }
    }
}