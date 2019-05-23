using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateLibrary
{
    public class MyPoint
    {
        public string Name { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return String.Format("Name: {0}, Value: {1}", Name, Value);
        }
    }
}
