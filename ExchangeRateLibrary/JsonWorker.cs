using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateLibrary
{
    public static class JsonWorker
    {
        public static List<String> GetCurrencies()
        {
            string json = new System.Net.WebClient().DownloadString("https://openexchangerates.org/api/currencies.json");

            var output = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            return output.Keys.ToList<string>();
        }

        public static ExchangeDataRoot GetExchangeData(string from, string to)
        {
            string http = String.Format("https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency={0}&to_currency={1}&apikey=WN02BP1Q1KZR95NO", from, to);

            string json = new System.Net.WebClient().DownloadString(http);

            ExchangeDataRoot output = JsonConvert.DeserializeObject<ExchangeDataRoot>(json);

            return output;
        }

        public static Dictionary<string, HistoricExchangeData> GetHistoricData(string from, string to)
        {
            Dictionary<string, HistoricExchangeData> output;

            string http = String.Format("https://www.alphavantage.co/query?function=FX_DAILY&from_symbol={0}&to_symbol={1}&apikey=WN02BP1Q1KZR95NO", from, to);

            string json = new System.Net.WebClient().DownloadString(http);

            var tmp = JsonConvert.DeserializeObject<HistoricExchangeDataRoot>(json);

            output = tmp.Result;

            return output;
        }

    }
}
