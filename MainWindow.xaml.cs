using System;
using System.Collections.Generic;
using System.Windows;

namespace ForeignExchangeConverter
{
    public partial class MainWindow : Window
    {
        private ExchangeRatesServiceAnswer exchangeRatesServiceAnswer;
        private Dictionary<string, double> exchangeRates;
        private CurrenciesServiceAnswer currenciesServiceAnswer;
        private Dictionary<string, string> currencies;

        public MainWindow()
        {
            InitializeComponent();
            GetExchangeRatesServiceAnswer();
            GetCurrenciesServiceAnswer();
        }

        private async void GetExchangeRatesServiceAnswer()
        {
            exchangeRatesServiceAnswer = await Service.GetExchangeRates();
            ConfigureExchangeRates();
        }

        private void ConfigureExchangeRates()
        {
            if (!exchangeRatesServiceAnswer.Error)
            {
                if (exchangeRatesServiceAnswer.ExchangeRate != null && exchangeRatesServiceAnswer.ExchangeRate.Rates != null)
                {
                    exchangeRates = exchangeRatesServiceAnswer.ExchangeRate.Rates;
                }
                else
                {
                    MessageBox.Show(exchangeRatesServiceAnswer.Message, "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(exchangeRatesServiceAnswer.Message, "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void GetCurrenciesServiceAnswer()
        {
            currenciesServiceAnswer = await Service.GetCurrencies();
            ConfigureCurrencies();
        }

        private void ConfigureCurrencies()
        {
            if (!currenciesServiceAnswer.Error)
            {
                if (currenciesServiceAnswer.Currencies != null)
                {
                    currencies = currenciesServiceAnswer.Currencies;
                    SourceExchangeRateComboBox.ItemsSource = currencies;
                    DestinationExchangeRateComboBox.ItemsSource = currencies;
                }
                else
                {
                    MessageBox.Show(currenciesServiceAnswer.Message, "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(currenciesServiceAnswer.Message, "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SimpleConvertButtonClick(object sender, RoutedEventArgs e)
        {
            string amountInText = SimpleAmountTextBox.Text;
            if (!string.IsNullOrEmpty(amountInText))
            {
                try
                {
                    double amount = Convert.ToDouble(amountInText);
                    if (exchangeRatesServiceAnswer.ExchangeRate.Rates.TryGetValue("MXN", out double mexicanPesosExchangeRate))
                    {
                        double foreignExchangeRateResult = (PesosToDollarsRadioButton.IsChecked == true) ? amount / mexicanPesosExchangeRate: amount * mexicanPesosExchangeRate;
                        SimpleForeignExchangeResultLabel.Content = string.Format("{0:#,##0.00}", foreignExchangeRateResult);
                        SimpleExchangeRateLabel.Content = string.Format("{0:#,##0.0000}", mexicanPesosExchangeRate);
                        SimpleDateAndTimeOfUpdateLabel.Content = GetDateAndTimeOfUpdate();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Los datos ingresados son inválidos.", "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("No se puede dejar ningún campo vacío.", "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private string GetDateAndTimeOfUpdate()
        {
            long dateAndTimeOfUpdate = exchangeRatesServiceAnswer.ExchangeRate.Timestamp;
            DateTimeOffset.Now.ToUnixTimeSeconds();
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(dateAndTimeOfUpdate).ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void ExchangeButtonClick(object sender, RoutedEventArgs e)
        {
            (DestinationExchangeRateComboBox.SelectedItem, SourceExchangeRateComboBox.SelectedItem) = 
                (SourceExchangeRateComboBox.SelectedItem, DestinationExchangeRateComboBox.SelectedItem);
        }

        private void ConvertButtonClick(object sender, RoutedEventArgs e)
        {
            string amountInText = AmountTextBox.Text;
            if (!string.IsNullOrEmpty(amountInText))
            {
                try
                {
                    string sourceExchangeRateKey = ((KeyValuePair<string, string>)SourceExchangeRateComboBox.SelectedValue).Key.ToString();
                    string destinationExchangeRateKey = ((KeyValuePair<string, string>)DestinationExchangeRateComboBox.SelectedValue).Key.ToString();
                    double amount = Convert.ToDouble(amountInText);
                    if (exchangeRates.TryGetValue(sourceExchangeRateKey, out double sourceExchangeRate) &&
                        exchangeRates.TryGetValue(destinationExchangeRateKey, out double destinationExchangeRate))
                    {
                        double foreignExchangeRateResult = sourceExchangeRate / destinationExchangeRate;
                        ExchangeRateLabel.Content = string.Format("{0:#,##0.0000}", foreignExchangeRateResult);
                        foreignExchangeRateResult = amount / foreignExchangeRateResult;
                        ForeignExchangeResultLabel.Content = string.Format("{0:#,##0.00}", foreignExchangeRateResult);
                        DateAndTimeOfUpdateLabel.Content = GetDateAndTimeOfUpdate();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Los datos ingresados son inválidos.", "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("No se puede dejar ningún campo vacío.", "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}