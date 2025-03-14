using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class ApplicationRole : IdentityRole<int>
{
    // Custom properties down here
    public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
    public virtual ICollection<ApplicationRoleClaim>? RoleClaims { get; set; }
}
