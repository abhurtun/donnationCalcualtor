using Moq;
using NUnit.Framework;

namespace GiftAidCalculator.Tests
{
    [TestFixture]
    class As_Donor_Tests
    {

        private Mock<ISuplementRepository> SupplementMock;
        private Mock<ITaxRateStore> TaxRateStoreMock;
        private IGiftAidCalculatorService calculatorService;

        [SetUp]
        public void beforeEachTests()
        {
            TaxRateStoreMock = new Mock<ITaxRateStore>();
            SupplementMock = new Mock<ISuplementRepository>();

            TaxRateStoreMock.Setup(x => x.GetTaxRate()).Returns(20m);
            calculatorService = new GiftAidCalculatorService(TaxRateStoreMock.Object, SupplementMock.Object);
        }

        [Test]
        public void Should_Calculate_GiftAid_Given_DonationAmount_And_TaxRate()
        {
            var donationAmount = 10;
            var expectedAmount = 2.5m;
            var taxRate = 20m;

            var giftAidAmount = calculatorService.CalculateGiftAid(donationAmount, taxRate);

            Assert.AreEqual(expectedAmount, giftAidAmount);
        }

        [Test]

        public void GetTaxRate_Should_Be_Callled_Once_From_TaxRateStore()
        {
            calculatorService.CalculateGiftAid(It.IsAny<decimal>(), It.IsAny<EventType>());

            TaxRateStoreMock.Verify(t => t.GetTaxRate(), Times.Once);
        }

        [Test]
        [TestCase(10, 15.5)]
        [TestCase(100, 16.9)]
        public void GiftAidAmount_Should_Be_Calculated_Correctly_With_Changing_TaxRates(decimal donationAmount, decimal taxRate)
        {
            TaxRateStoreMock.Setup(x => x.GetTaxRate()).Returns(taxRate);

            var giftAidAmount = calculatorService.CalculateGiftAid(donationAmount, It.IsAny<EventType>());

            var expected = calculatorService.CalculateRoundedGiftAid(giftAidAmount);
            Assert.AreEqual(expected, giftAidAmount);
        }

        [Test]
        [TestCase(1.316, 1.32)]
        [TestCase(2.557, 2.56)]
        public void GiftAidAmount_Should_Be_Rounded_Correctly(decimal amount, decimal expected)
        {
            var roundedGiftAidAmount = calculatorService.CalculateRoundedGiftAid(amount);

            Assert.AreEqual(expected, roundedGiftAidAmount);
        }

        [Test]
        [TestCase(1, 20.0, 5)]
        [TestCase(2.55, 20.0, 5)]
        public void GiftAidAmount_For_Running_Event_Should_Be_Calculated_And_Rounded_Correctly(decimal donationAmount, decimal taxRate, decimal supplement)
        {
            TaxRateStoreMock.Setup(x => x.GetTaxRate()).Returns(taxRate);
            SupplementMock.Setup(s => s.GetSupplement(EventType.running)).Returns(5);

            var roundedGiftAidAmount = calculatorService.CalculateGiftAid(donationAmount,EventType.running);

            var calculatedGiftAidAmount = calculatorService.CalculateGiftAid(donationAmount, taxRate, supplement);
            var expected = calculatorService.CalculateRoundedGiftAid(calculatedGiftAidAmount);

            Assert.AreEqual(expected, roundedGiftAidAmount);
        }
    }
}
