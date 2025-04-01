using Models.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class RegisterViewModel
    {
        public ArtsDto? Arts { get; set; }
        public PatientDto? PatientDto { get; set; }
        public UserDto? CreateUserDto { get; set; }
        public OuderVoogdDto? OuderVoogd { get; set; }
        public int TrajectId { get; set; }
    }
}
