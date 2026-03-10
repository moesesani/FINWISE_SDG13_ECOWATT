using EcoWatt.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // Nilagyan ng '?' para payagan ang null value sa constructor
    public string? RawPassword { get; set; }

    public List<EnergyLog> EnergyLogs { get; set; } = new List<EnergyLog>();
}