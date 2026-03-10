using EcoWatt.DAL;
using EcoWatt.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EcoWatt.BLL
{
    public class EnergyService
    {
        private readonly AppDbContext _context;

        public EnergyService()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        #region Security Logic
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public async Task<bool> RegisterUserAsync(string username, string password)
        {
            if (_context.Users.Any(u => u.Username == username)) return false;

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                RawPassword = password 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public User? VerifyLogin(string username, string password)
        {
            var hashedInput = HashPassword(password);
            return _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hashedInput);
        }
        #endregion

        #region Energy Log CRUD (Hinihingi ng Error List mo)
        public async Task AddEnergyLogAsync(EnergyLog log)
        {
            _context.EnergyLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public List<EnergyLog> GetUserLogs(int userId)
        {
            return _context.EnergyLogs
                           .Where(l => l.UserId == userId)
                           .OrderByDescending(l => l.DateLogged)
                           .ToList();
        }

        public async Task UpdateEnergyLogAsync(EnergyLog updatedLog)
        {
            var existingLog = await _context.EnergyLogs.FindAsync(updatedLog.Id);
            if (existingLog != null)
            {
                existingLog.ApplianceName = updatedLog.ApplianceName;
                existingLog.Wattage = updatedLog.Wattage;
                existingLog.HoursUsed = updatedLog.HoursUsed;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteLogAsync(int id)
        {
            var log = await _context.EnergyLogs.FindAsync(id);
            if (log != null)
            {
                _context.EnergyLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }

        public void ExportLogsToJson(int userId, string fileName)
        {
            var logs = GetUserLogs(userId);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(logs, options);
            File.WriteAllText(fileName, jsonString);
        }
        #endregion
    }
}