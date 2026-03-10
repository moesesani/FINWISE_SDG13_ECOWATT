using System;
using System.Windows;
using EcoWatt.BLL;

namespace EcoWatt.UI
{
    public partial class RegisterWindow : Window
    {
        private readonly EnergyService _service;

        public RegisterWindow()
        {
            InitializeComponent();
            _service = new EnergyService();
        }

        // TOGGLE LOGIC: Ipakita ang text characters
        private void ChkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            txtRegVisiblePassword.Text = txtRegPassword.Password;
            txtRegVisiblePassword.Visibility = Visibility.Visible;
            txtRegPassword.Visibility = Visibility.Collapsed;
        }

        // TOGGLE LOGIC: Itago ang text characters (bullets)
        private void ChkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            txtRegPassword.Password = txtRegVisiblePassword.Text;
            txtRegPassword.Visibility = Visibility.Visible;
            txtRegVisiblePassword.Visibility = Visibility.Collapsed;
        }

        // REGISTER LOGIC
        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtRegUsername.Text;

                // Kunin ang password base sa kung alin ang kasalukuyang nakabukas (Visible vs Hidden)
                string password = chkShowPassword.IsChecked == true
                    ? txtRegVisiblePassword.Text
                    : txtRegPassword.Password;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    lblRegMessage.Text = "⚠️ Please fill in all fields.";
                    return;
                }

                // Tatawagin ang BLL para mag-save
                bool success = await _service.RegisterUserAsync(username, password);

                if (success)
                {
                    MessageBox.Show("✅ Registration Successful! Please login.");
                    OpenLoginWindow();
                }
                else
                {
                    lblRegMessage.Text = "❌ Username already exists.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OpenLoginWindow();
        }

        private void OpenLoginWindow()
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}