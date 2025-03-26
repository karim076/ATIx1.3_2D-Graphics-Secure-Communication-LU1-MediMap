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

        [Required]
        [StringLength(50)]
        public string VoorNaam { get; set; } = string.Empty;
        public string AvatarNaam { get; set; } = string.Empty;
        public ICollection<LogBook>? logbook { get; set; }
        [Required]
        [StringLength(50)]
        public string AchterNaam { get; set; } = string.Empty;
        public string ArtsNaam { get; set; } = string.Empty;
        public string TrajectNaam { get; set; } = string.Empty;
        public string OuderVoogdNaam { get; set; } = string.Empty;
    }
}
