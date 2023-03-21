using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ForeignExchangeConverter
{
    public class Service
    {
        private static readonly string URL_BASE = "https://openexchangerates.org/api/";
        private static readonly string APP_ID = "e77f99c02f404d34a3631b67223d85e5";

        public static async Task<ExchangeRatesServiceAnswer> GetExchangeRates()
        {
            ExchangeRatesServiceAnswer exchangeRatesServiceAnswer = new ExchangeRatesServiceAnswer();
            using (var httpClient = new HttpClient())
            {
                HttpRequestMessage httpRequestMessage;
                HttpResponseMessage httpResponseMessage;
                try
                {
                    string url = string.Format("{0}latest.json?app_id={1}", URL_BASE, APP_ID);
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                    if (httpResponseMessage != null)
                    {
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            string content = await httpResponseMessage.Content.ReadAsStringAsync();
                            ExchangeRate exchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(content);
                            if (exchangeRate != null && exchangeRate.Disclaimer != null && exchangeRate.Rates != null)
                            {
                                exchangeRatesServiceAnswer.Error = false;
                                exchangeRatesServiceAnswer.ExchangeRate = exchangeRate;
                            }
                            else
                            {
                                exchangeRatesServiceAnswer.Error = true;
                                exchangeRatesServiceAnswer.Message = "Could not deserialize the response in JSON. Pleasy, try again.";
                            }
                        }
                        else
                        {
                            exchangeRatesServiceAnswer.Error = true;
                            HttpStatusCode httpStatusCode = httpResponseMessage.StatusCode;
                            exchangeRatesServiceAnswer.Message = string.Format("Error {0} - {1}", (int)httpStatusCode, httpStatusCode);
                        }
                    }
                    else
                    {
                        exchangeRatesServiceAnswer.Error = true;
                        exchangeRatesServiceAnswer.Message = "No response could be obtained from the web service. Please, try again.";
                    }
                }
                catch (Exception exception)
                {
                    exchangeRatesServiceAnswer.Error = true;
                    exchangeRatesServiceAnswer.Message = exception.Message;
                }
                return exchangeRatesServiceAnswer;
            }
        }

        public static async Task<CurrenciesServiceAnswer> GetCurrencies()
        {
            CurrenciesServiceAnswer currenciesServiceAnswer = new CurrenciesServiceAnswer();
            using (var httpClient = new HttpClient())
            {
                HttpRequestMessage httpRequestMessage;
                HttpResponseMessage httpResponseMessage;
                try
                {
                    string url = string.Format("{0}currencies.json?app_id={1}", URL_BASE, APP_ID);
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                    if (httpResponseMessage != null)
                    {
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            string content = await httpResponseMessage.Content.ReadAsStringAsync();
                            Dictionary<string, string> currencies = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                            if (currencies != null)
                            {
                                currenciesServiceAnswer.Error = false;
                                currenciesServiceAnswer.Currencies = currencies;
                            }
                            else
                            {
                                currenciesServiceAnswer.Error = true;
                                currenciesServiceAnswer.Message = "Could not deserialize the response in JSON. Pleasy, try again.";
                            }
                        }
                        else
                        {
                            currenciesServiceAnswer.Error = true;
                            HttpStatusCode httpStatusCode = httpResponseMessage.StatusCode;
                            currenciesServiceAnswer.Message = string.Format("Error {0} - {1}", (int)httpStatusCode, httpStatusCode);
                        }
                    }
                    else
                    {
                        currenciesServiceAnswer.Error = true;
                        currenciesServiceAnswer.Message = "No response could be obtained from the web service. Please, try again.";
                    }
                }
                catch (Exception exception)
                {
                    currenciesServiceAnswer.Error = true;
                    currenciesServiceAnswer.Message = exception.Message;
                }
                return currenciesServiceAnswer;
            }
        }
    }
}