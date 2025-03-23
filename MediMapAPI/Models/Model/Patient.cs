using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string VoorNaam { get; set; } = string.Empty;

        public string AvatarNaam { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string AchterNaam { get; set; } = string.Empty;

        [ForeignKey("OuderVoogd")]
        public int OuderVoogdId { get; set; }
        public OuderVoogd? OuderVoogd { get; set; }

        [ForeignKey("Traject")]
        public int TrajectId { get; set; }
        public Traject? Traject { get; set; }

        [ForeignKey("Arts")]
        public int? ArtsId { get; set; }
        public Arts? Arts { get; set; }

        public ICollection<LogBook>? LogBooks { get; set; } 
    }
}
