using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Dto
{
    public class ProfileInformationDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Naam { get; set; } = string.Empty;

        [Required]
        public DateTime GeboorteDatum { get; set; }

        [Required]
        public string NaamDokter { get; set; } = string.Empty;

        [Required]
        public string BehandelPlan { get; set; } = string.Empty;

        public DateTime AfspraakDatum { get; set; }

        public int PatientId { get; set; }
        public int ArtsId { get; set; }
    }
}
