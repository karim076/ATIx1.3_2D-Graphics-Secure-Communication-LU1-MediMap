using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Dto
{
    public class TrajectDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Naam { get; set; } = string.Empty;

        public ICollection<TrajectZorgMoment>? TrajectZorgMomenten { get; set; }

        //public ICollection<Patient>? Patients { get; set; }
    }
}
