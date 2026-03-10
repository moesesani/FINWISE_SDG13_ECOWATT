using System;

namespace EcoWatt.Models
{
    public class EnergyLog
    {
        public int Id { get; set; }
        public string ApplianceName { get; set; } = string.Empty;

        // Ito ang mga hinahanap na definition ng error mo kanina:
        public double Wattage { get; set; }
        public double HoursUsed { get; set; }

        public DateTime DateLogged { get; set; } = DateTime.Now;
        public int UserId { get; set; }
    }
}