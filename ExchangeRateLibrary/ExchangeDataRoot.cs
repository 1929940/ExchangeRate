using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateLibrary
{
    public class ExchangeDataRoot
    {
        [JsonProperty(PropertyName = "Realtime Currency Exchange Rate")]
        public ExchangeData exchangeData;
    }
}
