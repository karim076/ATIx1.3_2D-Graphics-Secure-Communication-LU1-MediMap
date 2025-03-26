using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Dto
{
    public class PatientDto
    {
        public int Id { get; set; }
        [Required] public string VoorNaam { get; set; }
        [Required] public string AchterNaam { get; set; }
        public string AvatarNaam { get; set; }
        public string OuderVoogdNaam { get; set; }
        public string ArtsNaam { get; set; }
        public string TrajectNaam { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public DateTime Afspraakatum { get; set; }
    }

}
