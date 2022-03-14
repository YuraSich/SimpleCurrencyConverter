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
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RateData _data;
        List<string> valutes;
        DispatcherTimer timer;
        ValutePickingPage vpp;
        ObservableCollection<Button> buttons;
        public MainWindow()
        {
            InitializeComponent();

            vpp = new ValutePickingPage();
            buttons = new ObservableCollection<Button>();


            try
            {
                _data = new RateData(@"https://www.cbr-xml-daily.ru/daily_json.js");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                this.Close();
                return;
            }


            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();

            valutes = _data.Rates.Valute.Keys.ToList();
            foreach (var v in valutes)
            {
                Button button = new Button();
                button.Content = v;
                buttons.Add(button);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            LValute.ItemsSource = valutes;
            RValute.ItemsSource = valutes;
            LValute.SelectedIndex = 0;
            RValute.SelectedIndex = 1;
            LValue.Text = "";
            RValue.Text = "";
            SetValuteNames();
        }

        private void SetValuteNames()
        {
            if(LValute.SelectedItem == null || RValute.SelectedItem == null) return;
            LValuteName.Content = _data.Rates.Valute[LValute.SelectedItem.ToString()].Name;
            RValuteName.Content = _data.Rates.Valute[RValute.SelectedItem.ToString()].Name;
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
            double v1 = _data.Rates.Valute[RValute.SelectedItem.ToString()].Value * _data.Rates.Valute[LValute.SelectedItem.ToString()].Nominal;
            double v2 = _data.Rates.Valute[LValute.SelectedItem.ToString()].Value * _data.Rates.Valute[RValute.SelectedItem.ToString()].Nominal;
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

        private void timer_Tick(object sender, EventArgs e)
        {
            TabControl.SelectedIndex = 1;
            timer.Stop();

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

        private void LValute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RValue.Text = String.Empty;
            ConvertValue();
            SetValuteNames();
        }

        private void RValute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LValue.Text = String.Empty;
            ConvertValue();
            SetValuteNames();
        }
    }
}
