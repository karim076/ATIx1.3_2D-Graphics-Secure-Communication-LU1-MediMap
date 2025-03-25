using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class OuderVoogd
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string VoorNaam { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string AchterNaam { get; set; } = string.Empty;

        public ICollection<Patient>? Patients { get; set; }
    }
}
