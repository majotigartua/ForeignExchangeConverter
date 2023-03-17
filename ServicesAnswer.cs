namespace ForeignExchangeConverter
{
    public class ServicesAnswer
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public ExchangeRates ExchangeRates { get; set; }
    }
}