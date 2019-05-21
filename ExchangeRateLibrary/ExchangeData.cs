using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateLibrary
{
    public class ExchangeData
    {
        [JsonProperty(PropertyName = "1. From_Currency Code")]
        public string From_Code { get; set; }

        [JsonProperty(PropertyName = "3. To_Currency Code")]
        public string To_Code { get; set; }

        [JsonProperty(PropertyName = "5. Exchange Rate")]
        public double ExchangeRate { get; set; }
    }
}
