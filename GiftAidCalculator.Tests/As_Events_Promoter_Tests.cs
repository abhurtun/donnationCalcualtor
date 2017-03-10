using Moq;
using NUnit.Framework;

namespace GiftAidCalculator.Tests
{
    [TestFixture]
    class As_Events_Promoter_Tests
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
        public void Should_Call_GetSupplementAmount()
        {
            calculatorService.CalculateGiftAid(It.IsAny<decimal>(), It.IsAny<EventType>());
            SupplementMock.Verify(c => c.GetSupplement(It.IsAny<EventType>()), Times.Once);
        }

        [Test]
        public void Should_Apply_5Percent_Supplement_ToDonation_ForRunningEvent()
        {
            SupplementMock.Setup(s => s.GetSupplement(EventType.running)).Returns(5);

            var giftAidAmount = calculatorService.CalculateGiftAid(10, EventType.running);
            var expected = calculatorService.CalculateGiftAid(10, 20, 5);

            Assert.AreEqual(expected, giftAidAmount);
        }

        [Test]
        public void Should_Apply_3PercentSupplement_ToDonation_ForSwimmingEvent()
        {    
            SupplementMock.Setup(s => s.GetSupplement(EventType.swimming)).Returns(3);
                       
            var giftAidAmount = calculatorService.CalculateGiftAid(10, EventType.swimming);
            var expected = calculatorService.CalculateGiftAid(10, 20, 3);

            Assert.AreEqual(expected, giftAidAmount);
        }

        [Test]
        public void Should_Not_ApplySupplement_ToDonation_ForAnyOtherEvent()
        {
            SupplementMock.Setup(s => s.GetSupplement(EventType.other)).Returns(0);
            
            var result = calculatorService.CalculateGiftAid(10, EventType.other);
            var expected = calculatorService.CalculateGiftAid(10, 20);

            Assert.AreEqual(expected, result);
        }

    }
}
