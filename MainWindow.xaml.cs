using System;
using System.Collections.Generic;
using System.Windows;

namespace ForeignExchangeConverter
{
    public partial class MainWindow : Window
    {
        private ExchangeRatesServiceAnswer exchangeRatesServiceAnswer;
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
        }

        private async void GetCurrenciesServiceAnswer()
        {
            currenciesServiceAnswer = await Service.GetCurrencies();
            ConfigureCurrencies();
        }

        private void ConfigureCurrencies()
        {
            if (currenciesServiceAnswer != null)
            {
                if (currenciesServiceAnswer.Currencies != null)
                {
                    currencies = currenciesServiceAnswer.Currencies;
                    foreach (var currencie in currencies)
                    {
                        SourceExchangeRateComboBox.Items.Add(currencie.Value.ToString());
                        DestinationExchangeRateComboBox.Items.Add(currencie.Value.ToString());
                    }
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
                    double amount = double.Parse(amountInText);
                    if (!exchangeRatesServiceAnswer.Error)
                    {
                        if (exchangeRatesServiceAnswer.ExchangeRate != null && exchangeRatesServiceAnswer.ExchangeRate.Rates != null)
                        {
                            if (exchangeRatesServiceAnswer.ExchangeRate.Rates.TryGetValue("MXN", out double mexicanPesosExchangeRate))
                            {
                                double exchangeRateResult;
                                if (DollarsToPesosRadioButton.IsChecked == true)
                                {
                                    exchangeRateResult = amount * mexicanPesosExchangeRate;
                                }
                                else
                                {
                                    exchangeRateResult = amount / mexicanPesosExchangeRate;
                                }
                                SimpleForeignExchangeResultLabel.Content = string.Format("{0:#,##0.00}", exchangeRateResult);
                                SimpleExchangeRateLabel.Content = string.Format("{0:#,##0.0000}", mexicanPesosExchangeRate);
                                SimpleDateAndTimeOfUpdateLabel.Content = GetDateAndTimeOfUpdate();
                            }
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
        }

        private double ConvertToDollars(double amount)
        {
            double foreignExchangeResult = 0;
            return foreignExchangeResult;
        }
    }
}