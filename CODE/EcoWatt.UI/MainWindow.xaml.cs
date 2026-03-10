using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EcoWatt.BLL;
using EcoWatt.Models;

namespace EcoWatt.UI
{
    public partial class MainWindow : Window
    {
        private readonly EnergyService _energyService = new EnergyService();
        // Dito natin itatago ang ID ng user na nag-login
        private int _currentUserId;

        // FIX: Constructor na tumatanggap ng userId mula sa LoginWindow
        public MainWindow(int userId)
        {
            InitializeComponent();
            _currentUserId = userId; // I-save ang logged-in user ID
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Ginagamit na ang _currentUserId sa halip na "1"
                var logs = _energyService.GetUserLogs(_currentUserId);
                dgEnergyLogs.ItemsSource = logs;

                // FR3: Visualization - Top 5 Energy Consumers
                if (logs != null && logs.Any())
                {
                    chartControl.ItemsSource = logs.OrderByDescending(l => l.Wattage).Take(5).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAppliance.Text))
                {
                    MessageBox.Show("Please enter an appliance name.");
                    return;
                }

                if (!double.TryParse(txtWattage.Text, out double watts) || !double.TryParse(txtHours.Text, out double hours))
                {
                    MessageBox.Show("Please enter valid numbers for Wattage and Hours.");
                    return;
                }

                btnSave.IsEnabled = false;

                if (btnSave.Content.ToString() == "Update Data")
                {
                    if (btnSave.Tag is int id)
                    {
                        var updatedLog = new EnergyLog
                        {
                            Id = id,
                            ApplianceName = txtAppliance.Text,
                            Wattage = watts,
                            HoursUsed = hours,
                            UserId = _currentUserId // Siguradong sa kanya pa rin ang log
                        };

                        await _energyService.UpdateEnergyLogAsync(updatedLog);
                        MessageBox.Show("Energy Log Updated!");
                        btnSave.Content = "Save Data";
                    }
                }
                else
                {
                    var log = new EnergyLog
                    {
                        ApplianceName = txtAppliance.Text,
                        Wattage = watts,
                        HoursUsed = hours,
                        DateLogged = DateTime.Now,
                        UserId = _currentUserId // Gamit ang dynamic ID
                    };

                    await _energyService.AddEnergyLogAsync(log);
                    MessageBox.Show("Energy Log Saved Successfully!");
                }

                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Operation failed: " + ex.Message);
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var log = (sender as Button)?.DataContext as EnergyLog;
            if (log != null)
            {
                var result = MessageBox.Show($"Burahin ang {log.ApplianceName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _energyService.DeleteLogAsync(log.Id);
                    LoadData();
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var log = (sender as Button)?.DataContext as EnergyLog;
            if (log != null)
            {
                txtAppliance.Text = log.ApplianceName;
                txtWattage.Text = log.Wattage.ToString();
                txtHours.Text = log.HoursUsed.ToString();
                btnSave.Content = "Update Data";
                btnSave.Tag = log.Id;
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = "EcoWatt_Backup.json";
                _energyService.ExportLogsToJson(_currentUserId, fileName);
                MessageBox.Show($"Data successfully exported to {fileName}", "JSON Export");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtAppliance.Clear();
            txtWattage.Clear();
            txtHours.Clear();
            btnSave.Tag = null;
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            // 1. Magtanong muna para sa siguradong user experience
            var result = MessageBox.Show("Sigurado ka bang gusto mong mag-logout?",
                                         "Logout Confirmation",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // 2. Buksan ang LoginWindow
                LoginWindow login = new LoginWindow();
                login.Show();

                // 3. Isara ang kasalukuyang MainWindow
                this.Close();
            }
        }
    }
}