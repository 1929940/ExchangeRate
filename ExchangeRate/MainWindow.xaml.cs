using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExchangeRateLibrary;

namespace ExchangeRate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Point> SqrtPoints { get; set; }
        public ObservableCollection<Point> LnPoints { get; set; }

        public MainWindow()
        {
            InitializeComponent();

        }

        private void DropDown_From_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string valFrom = DropDown_From.SelectedItem as string;
            string valTo = DropDown_To.SelectedItem as string;

            if (valFrom == null || valTo == null || (valFrom == valTo)) return;

            var tmp = JsonWorker.GetExchangeData(valFrom, valTo);


            lbl_From.Content = valFrom;
            lbl_To.Content = valTo;
            lbl_ExchangeRate.Content = tmp.exchangeData.ExchangeRate;
        }

        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            // Swaps two values between comboboxes

            int SelectedIndex = DropDown_From.SelectedIndex;
            DropDown_From.SelectedIndex = DropDown_To.SelectedIndex;
            DropDown_To.SelectedIndex = SelectedIndex;
        }

        private void Cbx_Trend_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> list = JsonWorker.GetCurrencies();

            DropDown_From.ItemsSource = list;
            DropDown_To.ItemsSource = list;
        }
    }
}
