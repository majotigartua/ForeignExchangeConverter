using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ForeignExchangeConverter
{
    public class ForeignExchangeServices
    {
        private static readonly string URL_BASE = "https://openexchangerates.org/api/";
        private static readonly string APP_ID = "e77f99c02f404d34a3631b67223d85e5";

        public static async Task<ServicesAnswer> GetExchangeRates()
        {
            ServicesAnswer servicesAnswer = new ServicesAnswer();
            using (var httpClient = new HttpClient())
            {
                HttpRequestMessage httpRequestMessage = null;
                HttpResponseMessage httpResponseMessage = null;
                try
                {
                    string url = string.Format("{0}latest.json?app_id={1}&prettyprint=false", URL_BASE, APP_ID);
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                    if (httpResponseMessage != null)
                    {
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            string content = await httpResponseMessage.Content.ReadAsStringAsync();
                            ExchangeRates exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(content);
                            if (exchangeRates != null) 
                            {
                                if (exchangeRates.Disclaimer != null && exchangeRates.Rates != null)
                                {
                                    servicesAnswer.Error = false;
                                    servicesAnswer.Message = "OK";
                                    servicesAnswer.ExchangeRates = exchangeRates;
                                }
                            }
                            else
                            {
                                servicesAnswer.Error = true;
                                servicesAnswer.Message = "Could not deserialize the response in JSON. Pleasy, try again";
                            }
                        }
                        else
                        {
                            servicesAnswer.Error = true;
                            HttpStatusCode httpStatusCode = httpResponseMessage.StatusCode;
                            servicesAnswer.Message = string.Format("Error: {0} - {1}", (int)httpStatusCode, httpStatusCode);
                        }
                    } 
                    else
                    {
                        servicesAnswer.Error = true;
                        servicesAnswer.Message = "No response could be obtained from the web service. Please, try again.";
                    }
                }
                catch (Exception exception)
                {
                    servicesAnswer.Error = true;
                    servicesAnswer.Message = exception.Message;
                }
            }
            return servicesAnswer;
        }
    }
}