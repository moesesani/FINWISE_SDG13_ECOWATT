using System.Linq;
using EcoWatt.DAL;
using EcoWatt.Models;
using EcoWatt.BLL; // Idagdag ito para matawag ang EnergyService
using System.Windows;

namespace EcoWatt.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var context = new AppDbContext())
            {
                // Siguraduhin na created ang database
                context.Database.EnsureCreated();

                // Kung walang User, gawa tayo ng isa (Admin)
                if (!context.Users.Any())
                {
                    // Tatawagin natin ang EnergyService para i-hash ang password
                    var service = new EnergyService();

                    var adminUser = new User
                    {
                        Username = "Admin",
                        // FR4: Imbes na .Password = "123", gagamit tayo ng PasswordHash
                        PasswordHash = service.HashPassword("123")
                    };

                    context.Users.Add(adminUser);
                    context.SaveChanges();
                }
            }
        }
    }
}