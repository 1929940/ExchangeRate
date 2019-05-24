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
        #region GetCurrenciesTest

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
        #endregion
        #region GetExchangeData

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

        [Theory]
        [InlineData("AAAA", "BBBB")]
        [InlineData("","")]
        public void GetExchangeData_ShouldFail_InvalidData(string from, string to)
        {
            var actual = JsonWorker.GetExchangeData(from, to);

            Assert.Null(actual);
        }


        [Theory]
        [InlineData("PLN", "JPY")]
        [InlineData("USD", "JPY")]
        [InlineData("PLN", "EUR")]
        public void GetExchangeData_ShouldWork_ReturnsPositiveNumber(string from, string to)
        {
            var actual = JsonWorker.GetExchangeData(from, to);

            Assert.True(actual.exchangeData.ExchangeRate > 0.0);
        }

        #endregion
        #region GetHistoricData
        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void GetHistoricData_ShouldWork_ApiStressTest(int loops)
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
        public void GetHistoricData_ShouldFail_ApiStressTest(int loops)
        {
            List<object> actual = new List<object>();

            for (int i = 0; i < loops; i++)
            {
                var tmp = JsonWorker.GetHistoricData("USD", "PLN");
                actual.Add(tmp);
            }
            Assert.Contains(null, actual);
        }

        [Theory]
        [InlineData("AAAA", "BBBB")]
        [InlineData("", "")]
        public void GetHistoricData_ShouldFail_InvalidData(string from, string to)
        {
            var actual = JsonWorker.GetHistoricData(from, to);

            Assert.Null(actual);
        }

        // What if Api has no data for the call? WRONG THIS ALWAYS RETURNS RIGHT

        [Theory]
        [InlineData("PLN", "AED")]
        [InlineData("BTC", "PLN")]
        public void GetHistoricData_ShouldFail_NoSuchData(string from, string to)
        {
            var actual = JsonWorker.GetHistoricData(from, to);

            Assert.Null(actual);
        }

        [Theory]
        [InlineData("PLN", "JPY")]
        [InlineData("USD", "JPY")]
        [InlineData("PLN", "EUR")]
        public void GetHistoricData_ShouldWork_KeysCountGreaterThanZero(string from, string to)
        {
            var actual = JsonWorker.GetHistoricData(from, to);

            Assert.NotEmpty(actual.Keys);
        }

        [Theory]
        [InlineData("PLN", "JPY")]
        [InlineData("USD", "JPY")]
        [InlineData("PLN", "EUR")]
        public void GetHistoricData_ShouldWork_ValuesCountGreaterThanZero(string from, string to)
        {
            var actual = JsonWorker.GetHistoricData(from, to);

            Assert.NotEmpty(actual.Values);
        }

        [Theory]
        [InlineData("PLN", "JPY")]
        [InlineData("PLN", "EUR")]
        public void GetHistoricData_ShouldWork_NoKeysAreNull(string from, string to)
        {
            var pairs = JsonWorker.GetHistoricData(from, to);

            List<string> tmp = new List<string>();

            foreach (var item in pairs.Keys)
            {
                tmp.Add(item);
            }

            Assert.DoesNotContain(null, tmp);
        }

        [Theory]
        [InlineData("PLN", "JPY")]
        [InlineData("USD", "JPY")]
        public void GetHistoricData_ShouldWork_NoValuesAreNull(string from, string to)
        {
            var actual = JsonWorker.GetHistoricData(from, to);

            Assert.DoesNotContain(null, actual.Values);
        }

        [Theory]
        [InlineData("PLN", "JPY")]
        [InlineData("USD", "JPY")]
        public void GetHistoricData_ShouldWork_OpenValueIsGreaterThanZero(string from, string to)
        {
            var tmp = JsonWorker.GetHistoricData(from, to);

            List<double> actual = new List<double>();

            foreach (var item in tmp.Values)
            {
                actual.Add(item.Open);
            }

            Assert.Contains(actual, x => (x > 0));
        }

        [Theory]
        [InlineData("PLN", "JPY")]
        [InlineData("USD", "JPY")]
        public void GetHistoricData_ShouldWork_CloseValueIsGreaterThanZero(string from, string to)
        {
            var tmp = JsonWorker.GetHistoricData(from, to);

            List<double> actual = new List<double>();

            foreach (var item in tmp.Values)
            {
                actual.Add(item.Close);
            }

            Assert.Contains(actual, x => (x > 0));
        }
        #endregion
        #region GetHistoricPoints
        [Theory]
        [InlineData(null,null)]
        [InlineData("AAAA", "BBBB")]
        public void GetHistoricPoints_ShouldWork_InvalidData(string from, string to)
        {
            var actual = JsonWorker.GetHistoricPoints(from, to);

            Assert.Null(actual);
        }

        [Theory]
        [InlineData("USD","PLN")]
        public void GetHistoricPoints_ShouldWork_ListNotEmpty(string from, string to)
        {
            var actual = JsonWorker.GetHistoricPoints(from, to);

            Assert.NotEmpty(actual);
        }

        [Theory]
        [InlineData("PLN", "EUR")]
        public void GetHistoricPoints_ShouldWork_NameNeverNull(string from, string to)
        {
            var myPoints = JsonWorker.GetHistoricPoints(from, to);

            List<string> actual = new List<string>();

            foreach (var item in myPoints)
            {
                actual.Add(item.Name);
            }
            Assert.DoesNotContain(actual, x => (x == null));
        }

        [Theory]
        [InlineData("PLN", "EUR")]
        public void GetHistoricPoints_ShouldWork_ValueAboveZero(string from, string to)
        {
            var myPoints = JsonWorker.GetHistoricPoints(from, to);

            List<double> actual = new List<double>();

            foreach (var item in myPoints)
            {
                actual.Add(item.Value);
            }
            Assert.Contains(actual, x => (x > 0.0));
        }
        #endregion
    }
}
