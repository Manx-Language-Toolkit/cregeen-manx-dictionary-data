using Cregeen;
using NUnit.Framework;

namespace Cregeen_Dictionary.Test;

public class DerivativeMarkingTest
{
    [Test]
    public void TestAll()
    {
        {
            string goodInput = "her skill, &c. A";
            var (marking, remaining) = DerivativeMarking.ParseDerivativeMarking(goodInput);
            Assert.That(marking, Is.Not.Null);
            Assert.That(marking!.Marking, Is.EqualTo("A"));
            Assert.That(remaining, Is.EqualTo("her skill, &c."));
        }
        
        {

            string badInput = "her skill, &c. ";
            var (marking, remaining) = DerivativeMarking.ParseDerivativeMarking(badInput);
            Assert.That(marking, Is.Null);
            Assert.That(remaining, Is.EqualTo("her skill, &c. "));
        }

        {
            string untrimmedInput = " v. taught, did teach, learned or did learn, did gain, or impart knowledge. Y. ";
            var (marking, remaining) = DerivativeMarking.ParseDerivativeMarking(untrimmedInput);
            Assert.That(marking?.Marking, Is.EqualTo("Y"));
            Assert.That(remaining, Is.EqualTo("v. taught, did teach, learned or did learn, did gain, or impart knowledge."));
        }
        
        {
            string untrimmedInput = " v. taught, did teach, learned or did learn, did gain, or impart knowledge. [Y]. ";
            var (marking, remaining) = DerivativeMarking.ParseDerivativeMarking(untrimmedInput);
            Assert.That(marking?.Marking, Is.EqualTo("Y"));
            Assert.That(remaining, Is.EqualTo("v. taught, did teach, learned or did learn, did gain, or impart knowledge."));
        }
    }
}