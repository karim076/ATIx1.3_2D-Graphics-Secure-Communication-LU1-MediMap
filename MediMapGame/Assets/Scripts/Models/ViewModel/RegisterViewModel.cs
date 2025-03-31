using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.ViewModel
{
    public class RegisterViewModel
    {
        public Arts Arts;
        [JsonProperty("PatientDto")]
        public Patient Patient;
        public CreateUserDto CreateUserDto;
        public OuderVoogd OuderVoogd;
        public int trajectId;

    }
}
