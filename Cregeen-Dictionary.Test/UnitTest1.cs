using System;
using Cregeen;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static Cregeen_Dictionary.Test.TestUtil.ResourceFetcher;

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

            // gheuley exists outside triangular braces: &lt;<b>gheul</b> or&gt; <b>gheuley</b>,<i> s. </i>his gyve or fetter.<br>
            // But it should not be under 'gheul'
            Assert.That(def.AllPossibleWords(), Does.Contain("gheulagh")); //gheul + -agh
            
            var gheul = def.Single(x => x.Word.Trim() == "gheul*");
            Assert.That(gheul.AllPossibleWords(), Does.Not.Contain("gheuley"));
        }
        
        [Test]
        public void BoldWordIsIncluded()
        {
            // When I was searching, I occasionally want to look up 'hroggal'. The HTML is:
            // 
            var html = GetTestData("hroggal");

            List<Definition> def = GetDefinitions(html);

            var dyHroggal = def.Single(x => x.Word.Trim() == "dy hroggal");
            Assert.That(dyHroggal.AllPossibleWords(), Does.Contain("hroggal"));
        }
        
        [Test]
        public void FaaseEndToEndTest()
        {
            // ensure that '&#8209;' is not encoded
            var html = GetTestData("faase");

            List<Definition> def = GetDefinitions(html);

            Assert.That(def.AllPossibleWords(), Does.Contain("follym-faase"));
            Assert.That(def.AllPossibleWords(), Does.Not.Contain("follym&#8209;faase"));
        }

        private static List<Definition> GetDefinitions(string html) => Headword.ConvertToDefinitions(html);

        private static string GetTestData(string resource) => GetResource($"Cregeen_Dictionary.Test.TestData.{resource}.html");
    }
}


public static class Extensions
{
    public static IEnumerable<String> AllPossibleWords(this IEnumerable<Definition> d)
    {
        return d.SelectMany(AllPossibleWords);
    }
    public static IEnumerable<String> AllPossibleWords(this Definition d)
    {
        return d.AllChildren.SelectMany(x => x.PossibleWords);
    }
}