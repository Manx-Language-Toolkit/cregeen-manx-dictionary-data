using Cregeen;
using NUnit.Framework;
using System.Linq;
using static Cregeen_Dictionary.Test.TestUtil.ResourceFetcher;

namespace Cregeen_Dictionary.Test
{
    [TestFixture]
    public class EndToEndTest
    {
        [Test]
        public void Aght()
        {
            var html = GetResource("Cregeen_Dictionary.Test.TestData.aght.html");

            var aght = Headword.FromHtmlUnsafe(html);


            // ReSharper disable CommentTypo
            /*
             * aght, s. m. art, skill, behaviour, demeanor, gait, plight, way; pl.  yn. 
                e haght, s. her skill, &c. A 
                gaght, v. act, behave;  agh;  ee;  in;  ins;  ym;  yms;  ys, 94. A 
                gaghtey, v. acting, behaving. A 
                aghtal, a. artful, skilful, dexterous, expert, mannerly. 
                s’aghtal, a. how skilful, artful, &c. A 
                s’aghtaley, a. id., comp. and sup. A
                neu aghtal, a. unskilful, awkward.
                aghtallys, s. m. artfulness, skilfulness. 
                neu-aghtallys, s. f. unskillfulness, &c.
                aght--baghee, s. m. manner of life, occupation; 2 Tim. iii. 10: Agh t’ou uss dy slane er hoiggal my ynsagh, my aght-baghee, my chiarail my chredjue, my hurranse-foddey, my ghraih, my veenid. But thou hast fully known my doctrine, manner of life, purpose, faith, longsuffering, charity, patience; Jonah i. 8: Insh dooin, ta shin guee ort, quoi by-chyndagh ta’n olk shoh er jeet orrin: cre ta dty aght beaghee? Tell us, we pray thee, for whose cause this evil is upon us; What is thine occupation? 
                aghterbee, adv. any way, any wise, any how, however. 
                naght, s. m. the way; with myr [sc. myr naght], like as, that as; a contraction of yn and aght.
                aghtys, s.
                drogh aghtys, s. f. ill behaviour, misdemeanor.
                */
            // ReSharper restore CommentTypo

            Assert.That(aght.Definition.PossibleWords, Is.EquivalentTo(new[] { "aght", "aghtyn" }));
            Assert.That(aght.Definition.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.SubstantiveMasculine, Abbreviation.HasPlural })); // TODO: needs the plural marked
            Assert.That(aght.Definition.EntryText, Is.EqualTo("art, skill, behaviour, demeanor, gait, plight, way;"));

            Definition Get(int k) => aght.Definition.Children.Skip(k).First();

            var eHaght = Get(0);
            Assert.That(eHaght.PossibleWords, Is.EquivalentTo(new[] { "e haght", "haght" }));
            Assert.That(eHaght.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.Substantive }));
            Assert.That(eHaght.EntryText, Is.EqualTo("her skill, &c. A"));
            // TODO: A ... at the end of a line shows that the word is a derivative or aspiration of one whose initial radically is A 

            var gaght = Get(1);
            Assert.That(gaght.PossibleWords, Is.EquivalentTo(new[] { "gaght", "gaghtagh", "gaghtee", "gaghtin", "gaghtins", "gaghtym", "gaghtyms", "gaghtys" }));
            Assert.That(gaght.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.Verb }));
            Assert.That(gaght.EntryText, Is.EqualTo("act, behave; A"));

            var gaghtey = Get(2);
            Assert.That(gaghtey.PossibleWords, Is.EquivalentTo(new[] { "gaghtey" }));
            Assert.That(gaghtey.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.Verb }));
            Assert.That(gaghtey.EntryText, Is.EqualTo("acting, behaving. A"));

            var aghtal = Get(3);
            Assert.That(aghtal.PossibleWords, Is.EquivalentTo(new[] { "aghtal" }));
            Assert.That(aghtal.Abbreviations, Is.EquivalentTo(new[] { Abbreviation.Adjective }));
            Assert.That(aghtal.EntryText, Is.EqualTo("artful, skilful, dexterous, expert, mannerly."));

           Assert.Inconclusive();
        }


    }
}
