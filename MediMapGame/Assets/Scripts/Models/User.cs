using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class User
    {
    }

    public class UserDto
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int PatientPathLocation { get; set; }

        public int? PatienId { get; set; }

        public Patient? Patient { get; set; }

        public int? TrajectId { get; set; }
        public Traject? Traject { get; set; }
    }
}
