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
        [JsonProperty(PropertyName = "5. Exchange Rate")]
        public double ExchangeRate { get; set; }
    }
}
