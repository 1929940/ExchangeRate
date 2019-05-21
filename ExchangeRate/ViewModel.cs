using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ExchangeRate
{
    class ViewModel
    {
        public ObservableCollection<Point> SqrtPoints { get; set; }
        public ObservableCollection<Point> LnPoints { get; set; }

        public ViewModel()
        {
            var numbers = Enumerable.Range(0, 9 * 2 + 1).Select(x => x / 2.0);
            var sqrtData = numbers.Select(x => new Point { X = x, Y = Math.Pow(x, 0.5) });
            this.SqrtPoints = new ObservableCollection<Point>(sqrtData);

            //ln(0) is NaN
            var lnData = numbers.Where(x => x != 0).Select(x => new Point { X = x, Y = Math.Log(x) });
            this.LnPoints = new ObservableCollection<Point>(lnData);
        }
        public void Increase()
        {
            var numbers = Enumerable.Range(0, 9 * 3 + 1).Select(x => x);
            var sqrtData = numbers.Select(x => new Point { X = x, Y = Math.Pow(x, 0.5) });
            SqrtPoints = new ObservableCollection<Point>(sqrtData);
        }


        public class Point
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
