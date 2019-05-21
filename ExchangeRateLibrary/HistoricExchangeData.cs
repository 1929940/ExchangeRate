using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateLibrary
{
    public class HistoricExchangeData
    {
        [JsonProperty(PropertyName = "1. Open")]
        public double Open { get; set; }

        [JsonProperty(PropertyName = "4. close")]
        public double Close { get; set; }
    }
}
