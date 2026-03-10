using System;
using System.Windows;
using System.Windows.Media;
using EcoWatt.BLL;

namespace EcoWatt.UI
{
    public partial class LoginWindow : Window
    {
        private readonly EnergyService _service;

        public LoginWindow()
        {
            InitializeComponent();
            _service = new EnergyService();
        }

        // LOGIN LOGIC
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUsername.Text;

                // Kunin ang password kung alinman ang visible (PasswordBox o TextBox)
                string password = chkShowPassword.IsChecked == true
                                  ? txtVisiblePassword.Text
                                  : txtPassword.Password;

                var user = _service.VerifyLogin(username, password);

                if (user != null)
                {
                    MainWindow main = new MainWindow(user.Id);
                    main.Show();
                    this.Close();
                }
                else
                {
                    lblMessage.Foreground = Brushes.Red;
                    lblMessage.Text = "❌ INITIALIZATION FAILED: Invalid Operator Credentials.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"VOLTAGE ERROR: {ex.Message}");
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow regWindow = new RegisterWindow();
            regWindow.Show();
            this.Close();
        }

        // --- FIXED: Dagdag na functions para sa Show/Hide Password ---

        private void ChkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            // Ilipat ang laman ng PasswordBox papunta sa TextBox
            txtVisiblePassword.Text = txtPassword.Password;

            // I-switch ang visibility
            txtVisiblePassword.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Collapsed;
        }

        private void ChkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            // Ilipat ang laman ng TextBox pabalik sa PasswordBox
            txtPassword.Password = txtVisiblePassword.Text;

            // I-switch ang visibility pabalik
            txtPassword.Visibility = Visibility.Visible;
            txtVisiblePassword.Visibility = Visibility.Collapsed;
        }
    }
}