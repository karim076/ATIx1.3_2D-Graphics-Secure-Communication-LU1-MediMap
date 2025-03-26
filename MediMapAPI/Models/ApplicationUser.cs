using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Models.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class ApplicationUser : IdentityUser<int>
{
    public virtual ICollection<ApplicationUserClaim>? Claims { get; set; }
    public virtual ICollection<ApplicationUserLogin>? Logins { get; set; }
    public virtual ICollection<ApplicationUserToken>? Tokens { get; set; }
    public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }

    // Voeg je eigen properties hieronder toe die nog niet door IdentityUser wordt ondersteund
    public string? Name { get; set; }

    public int PatienId { get; set; }

    [ForeignKey(nameof(PatienId))]
    public Patient? Patient { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}
