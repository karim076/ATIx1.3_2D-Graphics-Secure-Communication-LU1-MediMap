namespace MediMapAPI.Service
{
    using global::Models.Model;
    using MediMapAPI.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Methode om een gebruiker aan te maken
        public async Task<IdentityResult> CreateUserAsync(string username, string email, string password, string role)
        {
            // Controleer of de rol bestaat
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                throw new Exception($"Role '{role}' does not exist.");
            }

            // Maak de gebruiker aan
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                NormalizedUserName = username.ToUpper(),
                NormalizedEmail = email?.ToUpper()
            };

            var result = await _userManager.CreateAsync(user, password);

            // Wijs de rol toe als de gebruiker succesvol is aangemaakt
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return result;
        }
    }
}
