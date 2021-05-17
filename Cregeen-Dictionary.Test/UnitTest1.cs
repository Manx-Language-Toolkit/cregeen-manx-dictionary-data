using Cregeen;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cregeen_Dictionary.Test
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var html = GetResource("Cregeen_Dictionary.Test.TestData.aght.html");
            
            List<Definition> def = GetDefinitions(html);

            Assert.That(def.First().PossibleWords.First(), Is.EqualTo("aght"));
        }

        [Test]
        public void TestDoubleInstance()
        {
            // two instances of `Times New Roman",serif`
            var html = GetResource("Cregeen_Dictionary.Test.TestData.annoon.html");

            List<Definition> def = GetDefinitions(html);

            // TODO: This should be an exact match
            Assert.That(def.First().PossibleWords.First(), Does.StartWith("annoon"));
        }

        [Test]
        public void TestClosingTags()
        {
            var html = GetResource("Cregeen_Dictionary.Test.TestData.bing.html");

            var headword = Headword.FromHtmlUnsafe(html);

            var json = OutDef.FromDef(headword);

            Assert.That(json.EntryHtml.EndsWith("</b>"));
        }

        [Test]
        public void SicIsStripped()
        {
            // two instances of `Times New Roman",serif`
            var html = GetResource("Cregeen_Dictionary.Test.TestData.annoon.html");

            List<Definition> def = GetDefinitions(html);

            Assert.That(def.First().PossibleWords.First(), Is.EqualTo("annoon"));
        }

        [Test]
        public void BracesAreStripped()
        {
            // [*] goes to *
            var html = GetResource("Cregeen_Dictionary.Test.TestData.aggle.html");

            List<Definition> def = GetDefinitions(html);

            Assert.That(def.SelectMany(x => x.AllChildren.SelectMany(x => x.PossibleWords)), Does.Contain("cha n'agglagh"));
        }

        [Test]
        public void TriangularBracesAreStripped()
        {
            // "I indicate elements for suppression between < >, "
            var html = GetResource("Cregeen_Dictionary.Test.TestData.gheul.html");
            // <b>gheul</b>* &lt;or <b>gheuley</b>&gt; - so this should just be 'gheul' without 'gheuley'"

            List<Definition> def = GetDefinitions(html);

            Assert.That(def.SelectMany(x => x.AllChildren.SelectMany(x => x.PossibleWords)), Does.Contain("gheulagh")); //gheul + -agh
            Assert.That(def.SelectMany(x => x.AllChildren.SelectMany(x => x.PossibleWords)), Does.Not.Contain("gheuley"));
        }

        private static List<Definition> GetDefinitions(string html)
        {
            return Headword.ConvertToDefinitions(html);
        }

        private string GetResource(string file)
        {
            using var stream = typeof(Tests).Assembly.GetManifestResourceStream(file);
            
            if (stream == null)
            {
                var names = typeof(Tests).Assembly.GetManifestResourceNames();
            }

            using TextReader tr = new StreamReader(stream);
            return tr.ReadToEnd();
            
        }
    }
}