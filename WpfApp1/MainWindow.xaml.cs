using System;
using System.Collections.Generic;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RateData _data;
        List<string> valutes;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _data = new RateData(@"https://www.cbr-xml-daily.ru/daily_json.js");
            valutes = _data.Rates.Valute.Keys.ToList();
            LButton.Content = valutes[0];
            RButton.Content = valutes[1];
            LValue.Text = "";
            RValue.Text = "";
            ConvertValue();
        }

        private void ConvertValue()
        {
            double lv, rv;
            if (LValue.Text != String.Empty && !double.TryParse(LValue.Text, out lv)) return;
            if (RValue.Text != String.Empty && !double.TryParse(RValue.Text, out rv)) return;
            if (LValue.Text == String.Empty && RValue.Text == String.Empty)
            {
                return;
            }
            double v1 = _data.Rates.Valute[RButton.Content.ToString()].Value * _data.Rates.Valute[LButton.Content.ToString()].Nominal;
            double v2 = _data.Rates.Valute[LButton.Content.ToString()].Value * _data.Rates.Valute[RButton.Content.ToString()].Nominal;
            double cur = LValue.Text == String.Empty ? double.Parse(RValue.Text) : double.Parse(LValue.Text);
            if (LValue.Text == String.Empty)
            {
                LValue.Text = (cur * v1 / v2).ToString("F2");
            }
            if (RValue.Text == String.Empty)
            {
                RValue.Text = (cur * v2 / v1).ToString("F2");
            }
        }

        private void LValue_KeyUp(object sender, KeyEventArgs e)
        {
            RValue.Text = String.Empty;
            ConvertValue();
        }

        private void RValue_KeyUp(object sender, KeyEventArgs e)
        {
            LValue.Text = String.Empty;
            ConvertValue();
        }
    }
}
