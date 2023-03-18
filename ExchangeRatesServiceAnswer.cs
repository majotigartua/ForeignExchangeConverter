namespace ForeignExchangeConverter
{
    public class ExchangeRatesServiceAnswer
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public ExchangeRate ExchangeRate { get; set; }
    }
}