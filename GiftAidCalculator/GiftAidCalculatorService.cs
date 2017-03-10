using System;

namespace GiftAidCalculator
{
    public class GiftAidCalculatorService : IGiftAidCalculatorService
    {
        private readonly ITaxRateStore _taxRateStore;
        private readonly ISuplementRepository _suplementRepository;

        public GiftAidCalculatorService(ITaxRateStore taxRateStore, ISuplementRepository suplementRepository)
        {
            _taxRateStore = taxRateStore;
            _suplementRepository = suplementRepository;
        }

        public decimal CalculateGiftAid(decimal donationAmount, decimal taxRate)
        {
            return donationAmount * (taxRate / (100 - taxRate));
        }

        public decimal CalculateGiftAid(decimal donationAmount, decimal taxRate, decimal supplement)
        {
            return CalculateGiftAid(donationAmount, taxRate) * supplement;
        }


        public decimal CalculateGiftAid(decimal donationAmount, EventType eventType)
        {
            var taxRate = _taxRateStore.GetTaxRate();
            var supplement = _suplementRepository.GetSupplement(eventType);

            var calculatedAmount = eventType == EventType.other ? 
                CalculateGiftAid(donationAmount, taxRate) : 
                CalculateGiftAid(donationAmount, taxRate, supplement);

            return CalculateRoundedGiftAid(calculatedAmount);
        }

        public decimal CalculateRoundedGiftAid(decimal giftAidAmount)
        {
            return Math.Round(giftAidAmount, 2);
        }
    }
}