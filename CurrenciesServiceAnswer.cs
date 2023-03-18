using System.Collections.Generic;

namespace ForeignExchangeConverter
{
    public class CurrenciesServiceAnswer
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Currencies { get; set; }
    }
}