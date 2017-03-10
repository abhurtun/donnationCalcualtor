namespace GiftAidCalculator
{
    public interface IGiftAidCalculatorService
    {
        decimal CalculateGiftAid(decimal donationAmount, decimal taxRate, decimal supplement);
        decimal CalculateGiftAid(decimal donationAmount, EventType eventType);
        decimal CalculateGiftAid(decimal donationAmount, decimal taxRate);
        decimal CalculateRoundedGiftAid(decimal giftAidAmount);
    }
}