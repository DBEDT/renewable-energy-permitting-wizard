using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HawaiiDBEDT.Web.utils;
using NUnit.Framework;

namespace Tests.utils
{
    [TestFixture]
    public class HtmlEncoderTests
    {
        [Test]
        public void BetterHtmlDecodeAmpersandTest()
        {
            string text = "Harry &amp; Larry";
            string result = HtmlEncoder.BetterHtmlDecode(text);
            Assert.AreEqual(result, "Harry & Larry");
        }

        [Test]
        public void BetterHtmlDecodeWordQuotationTest()
        {
            string text = "Facility is a &ldquo;permissible use&quot; on such lands";
            string result = HtmlEncoder.BetterHtmlDecode(text);
            Assert.AreEqual(result, "Facility is a “permissible use\" on such lands");
        }

        [Test]
        public void BetterHtmlDecodeApostropheTest()
        {
            string text = "The Land Study Bureau&apos;s soil classification";
            string result = HtmlEncoder.BetterHtmlDecode(text);
            Assert.AreEqual(result, "The Land Study Bureau's soil classification");
        }

        [Test]
        public void BetterHtmlDecodeSectionSignTest()
        {
            string text = "See &sect;205";
            string result = HtmlEncoder.BetterHtmlDecode(text);
            Assert.AreEqual(result, "See §205");
        }

        [Test]
        public void BetterHtmlDecodeDollarSignTest()
        {
            string text = "$20,000.00";
            string result = HtmlEncoder.BetterHtmlDecode(text);
            Assert.AreEqual(result, "$20,000.00");
        }

        [Test]
        public void BetterHtmlDecodeNumericAmpersandTest()
        {
            string text = "Harry &#38; Larry";
            string result = HtmlEncoder.BetterHtmlDecode(text);
            Assert.AreEqual(result, "Harry & Larry");
        }

        [Test]
        public void BetterHtmlDecodeNumericWordQuotationTest()
        {
            string text = "Facility is a &#8220;permissible use&quot; on such lands";
            string result = HtmlEncoder.BetterHtmlDecode(text);
            Assert.AreEqual(result, "Facility is a “permissible use\" on such lands");
        }

        [Test]
        public void BetterHtmlDecodeNumericApostropheTest()
        {
            string text = "The Land Study Bureau&#39;s soil classification";
            string result = HtmlEncoder.BetterHtmlDecode(text);
            Assert.AreEqual(result, "The Land Study Bureau's soil classification");
        }

        [Test]
        public void BetterHtmlDecodeNumericDollarSignTest()
        {
            string text = "&#36;20,000.00";
            string result = HtmlEncoder.BetterHtmlDecode(text);
            Assert.AreEqual(result, "$20,000.00");
        }

        [Test]
        public void BetterHtmlDecodeNumericSectionSignTest()
        {
            string text = "See &sect;205";
            string result = HtmlEncoder.BetterHtmlDecode(text);
            Assert.AreEqual(result, "See §205");
        }
    }
}
