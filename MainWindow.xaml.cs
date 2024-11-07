using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MyApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer updateTimer;
        private DispatcherTimer cooldownTimer;
        private DateTime lastScanTime;
        private TimeSpan scanCooldown = TimeSpan.FromMinutes(5);

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimers();
            LoadAsteroidDataAsync();
        }

        private void InitializeTimers()
        {
            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMinutes(20);
            updateTimer.Tick += (sender, e) => LoadAsteroidDataAsync();
            updateTimer.Start();

            cooldownTimer = new DispatcherTimer();
            cooldownTimer.Interval = TimeSpan.FromSeconds(1);
            cooldownTimer.Tick += CooldownTimer_Tick;
            cooldownTimer.Start();
        }

        private async Task LoadAsteroidDataAsync()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync("https://api.nasa.gov/neo/rest/v1/neo/browse?api_key=DEMO_KEY");
            var asteroidData = JsonConvert.DeserializeObject<NeoWsResponse>(response);
            AsteroidListView.ItemsSource = asteroidData.near_earth_objects;
        }

        private async void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateTime.Now - lastScanTime >= scanCooldown)
            {
                lastScanTime = DateTime.Now;
                await LoadAsteroidDataAsync();
            }
        }

        private void CooldownTimer_Tick(object sender, EventArgs e)
        {
            var remainingTime = scanCooldown - (DateTime.Now - lastScanTime);
            if (remainingTime <= TimeSpan.Zero)
            {
                CooldownTextBlock.Text = "Zbývající čas do dalšího skenu: 0 minut";
            }
            else
            {
                CooldownTextBlock.Text = $"Zbývající čas do dalšího skenu: {remainingTime.Minutes} minut a {remainingTime.Seconds} sekund";
            }
        }

        private void AsteroidListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAsteroid = (NearEarthObject)AsteroidListView.SelectedItem;
            if (selectedAsteroid != null)
            {
                DetailTextBlock.Text = $"Název: {selectedAsteroid.name}\n" +
                                       $"Průměr (km): {selectedAsteroid.estimated_diameter.kilometers.estimated_diameter_max}\n" +
                                       $"Nebezpečný: {selectedAsteroid.is_potentially_hazardous_asteroid}\n" +
                                       $"Odkaz na databázi NASA: [Link](https://ssd.jpl.nasa.gov/tools/sbdb_lookup.html#/?sstr={selectedAsteroid.name})";
            }
        }
    }

    public class NeoWsResponse
    {
        public List<NearEarthObject> near_earth_objects { get; set; }
    }

    public class NearEarthObject
    {
        public string name { get; set; }
        public bool is_potentially_hazardous_asteroid { get; set; }
        public EstimatedDiameter estimated_diameter { get; set; }
    }

    public class EstimatedDiameter
    {
        public Kilometers kilometers { get; set; }
    }

    public class Kilometers
    {
        public double estimated_diameter_max { get; set; }
    }
}
