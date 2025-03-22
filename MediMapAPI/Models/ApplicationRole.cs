using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class ApplicationRole : IdentityRole<int>
{
    public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
    public virtual ICollection<ApplicationRoleClaim>? RoleClaims { get; set; }

    // voeg je eigen properties hieronder toe die nog niet door IdentityRole wordt ondersteund
}
