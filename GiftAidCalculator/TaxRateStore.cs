namespace GiftAidCalculator
{
    public class TaxRateStore : ITaxRateStore
    {
        public decimal CurrentTaxRate;
        public void SetTaxRate(decimal taxRate)
        {
            CurrentTaxRate = taxRate;
        }

        public decimal GetTaxRate()
        {
            return CurrentTaxRate;
        }
    }
}