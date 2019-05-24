using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateLibrary
{
    public class HistoricExchangeDataRoot
    {
        [JsonProperty(PropertyName = "Time Series FX (Daily)")]
        public Dictionary<DateTime, HistoricExchangeData> Result { get; set; }
    }
}
