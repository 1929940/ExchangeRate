using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateLibrary;
using Xunit;

namespace ExchangeRate.Test
{
    public class JsonWorkerTest
    {
        // What happens if we make many unneeded calls to the API? 

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void GetCurrencies_ShouldWork_CheckNumberOfCalls(int loops)
        {
            int counter = 0;

            for (int i = 0; i < loops; i++)
            {
                int actual = JsonWorker.GetCurrencies().Count;
                counter++;
                Assert.True(actual > 0);
            }
        }
        // Is the order always the same? 

        [Theory]
        [InlineData(0, "AED")]
        [InlineData(46, "EUR")]
        [InlineData(116, "PLN")]
        public void GetCurrencies_ShouldWork_CheckIfOrderChanges(int index, string expected)
        {
            // Arrange

            // Act

            var actual = JsonWorker.GetCurrencies();

            // Assert

            Assert.Equal(expected, actual[index]);
        }

        [Theory]
        [InlineData(0, "EUR")]
        [InlineData(46, "BTC")]
        [InlineData(116, "BTCX")]
        public void GetCurrencies_ShouldFail_CheckIfOrderChanges(int index, string expected)
        {
            // Arrange

            // Act

            var actual = JsonWorker.GetCurrencies();

            // Assert

            Assert.NotEqual(expected, actual[index]);
        }
        [Fact]
        public void GetCurrencies_ShouldWork_AreAllCodesThreeDigitLong()
        {
            // Arrange

            // Act

            var actual = JsonWorker.GetCurrencies();

            // Assert

            Assert.DoesNotContain(actual, x => (x.Length > 3));
        }



        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void GetExchangeData_ShouldWork_ApiStressTest(int loops)
        {
            List<object> actual = new List<object>();

            for (int i = 0; i < loops; i++)
            {
                var tmp = JsonWorker.GetExchangeData("USD", "PLN");
                actual.Add(tmp);
            }
            Assert.DoesNotContain(null, actual);
        }

        [Theory]
        [InlineData(10)]
        public void GetExchangeData_ShouldFail_ApiStressTest(int loops)
        {
            List<object> actual = new List<object>();

            for (int i = 0; i < loops; i++)
            {
                var tmp = JsonWorker.GetExchangeData("USD", "PLN");
                actual.Add(tmp);
            }
            Assert.Contains(null, actual);
        }

        [Fact]
        public void GetExchangeData_ShouldFail_InvalidData()
        {
            // Arrange

            string one = "AAAA";
            string two = "BBBB";

            var actual = JsonWorker.GetExchangeData(one, two);

            Assert.Null(actual);
        }

        // What if Api has no data for the call? 

        [Theory]
        [InlineData("PLN", "AED")]
        [InlineData("BTC", "PLN")]
        public void GetExchangeData_ShouldFail_NoSuchData(string from, string to)
        {
            var actual = JsonWorker.GetExchangeData(from, to);

            Assert.Null(actual);
        }


    }
}
