using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Dto
{
    public class CreateUserDto
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        [Required] public int PatientPathLocation { get; set; }

        public int? PatienId { get; set; }

        public Patient? Patient { get; set; }

        public int? TrajectId { get; set; }
        public Traject? Traject { get; set; }
    }
}
