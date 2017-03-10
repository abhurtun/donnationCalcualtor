using NUnit.Framework;

namespace GiftAidCalculator.Tests
{
    [TestFixture]
    class As_Site_Adminsitrator_Tests
    {
        [Test]
        public void Should_Change_TaxRate_To15()
        {
            var taxRate = 15m;

            var sut =  new TaxRateStore();

            sut.SetTaxRate(taxRate);

            Assert.AreEqual(taxRate, sut.CurrentTaxRate);
        }
    }
}
