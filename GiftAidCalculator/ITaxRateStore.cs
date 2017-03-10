namespace GiftAidCalculator
{
    public interface ITaxRateStore
    {
        void SetTaxRate(decimal taxRate);

        decimal GetTaxRate();
    }
}