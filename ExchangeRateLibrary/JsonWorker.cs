using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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

            // Alphavantage API calls are limited to 5 per 30 seconds.
            if (json.Contains("Note") || (json.Contains("Error Message")))
            {
                System.Diagnostics.Debug.WriteLine(json);
                return null;
            }

            ExchangeDataRoot output = JsonConvert.DeserializeObject<ExchangeDataRoot>(json);

            return output;
        }

        public static Dictionary<DateTime, HistoricExchangeData> GetHistoricData(string from, string to)
        {
            Dictionary<DateTime, HistoricExchangeData> output;

            string http = String.Format("https://www.alphavantage.co/query?function=FX_DAILY&from_symbol={0}&to_symbol={1}&apikey=WN02BP1Q1KZR95NO", from, to);

            string json = new System.Net.WebClient().DownloadString(http);

            // API Alphavantage calls are limited to 5 per 30 seconds. 
            if (json.Contains("Note") || (json.Contains("Error Message")))
            {
                System.Diagnostics.Debug.WriteLine(json);
                return null;
            }
            var tmp = JsonConvert.DeserializeObject<HistoricExchangeDataRoot>(json);

            output = tmp.Result;

            return output;
        }

        public static List<MyPoint> GetHistoricPoints(string from, string to)
        {
            var dict = GetHistoricData(from, to);

            List<MyPoint> output = new List<MyPoint>();

            if (dict == null) return null;

            foreach (var item in dict.Keys)
            {
                //MyPoint Open = new MyPoint();
                MyPoint Close = new MyPoint();

                //Open.Date = item;
                //Open.Value = dict[item].Open;

                Close.Date = item;
                Close.Value = dict[item].Close;

                //output.Add(Open);
                output.Add(Close);
            }

            return output;
        }

    }
}
