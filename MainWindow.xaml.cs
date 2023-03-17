using System;
using System.Windows;

namespace ForeignExchangeConverter
{
    public partial class MainWindow : Window
    {
        private ServicesAnswer servicesAnswer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ConvertButtonClick(object sender, RoutedEventArgs e)
        {
            string amountInText = AmountTextBox.Text;
            if (!string.IsNullOrEmpty(amountInText))
            {
                try
                {
                    double amount = double.Parse(amountInText);
                    servicesAnswer = await ForeignExchangeServices.GetExchangeRates();
                    if (!servicesAnswer.Error)
                    {
                        if (servicesAnswer.ExchangeRates != null && servicesAnswer.ExchangeRates.Rates != null)
                        {
                            if (servicesAnswer.ExchangeRates.Rates.TryGetValue("MXN", out double pesosExchangeRate))
                            {
                                if (DollarsToPesosRadioButton.IsChecked == true)
                                {
                                    ForeignExchangeResultLabel.Content = string.Format("{0:#,##0.00}", amount / pesosExchangeRate);
                                }
                                else
                                {
                                    ForeignExchangeResultLabel.Content = string.Format("{0:#,##0.00}", amount * pesosExchangeRate);
                                }
                                ConfigureDateAndTimeOfUpdate();
                                ExchangeRateLabel.Content = string.Format("{0:#,##0.0000}", pesosExchangeRate);
                            }
                        }
                        else
                        {
                            MessageBox.Show(servicesAnswer.Message, "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(servicesAnswer.Message, "ADVERTENCIA", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void ConfigureDateAndTimeOfUpdate()
        {
            long dateAndTimeOfUpdate = servicesAnswer.ExchangeRates.Timestamp;
            DateTimeOffset.Now.ToUnixTimeSeconds();
            DateAndTimeOfUpdateLabel.Content = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(dateAndTimeOfUpdate).ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}