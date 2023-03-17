using System.Collections.Generic;

namespace ForeignExchangeConverter
{
    public class ExchangeRates
    {
        public string Disclaimer { get; set; }
        public string License { get; set; }
        public long Timestamp { get; set; }
        public string Base { get; set; }
        public Dictionary<string, double> Rates { get; set; }
    }
}