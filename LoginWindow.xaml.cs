using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace NASApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string token = TokenTextBox.Text;
            if (!string.IsNullOrEmpty(token))
            {
                bool isValidToken = await VerifyTokenAsync(token);
                if (isValidToken)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Neplatný token.");
                }
            }
            else
            {
                MessageBox.Show("Prosím, zadejte token.");
            }
        }

        private async Task<bool> VerifyTokenAsync(string token)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync($"https://api.nasa.gov/neo/rest/v1/neo/browse?api_key={token}");
                return !string.IsNullOrEmpty(response);
            }
            catch
            {
                return false;
            }
        }
    }
}
