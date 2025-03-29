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

        public DateTime GeboorteDatum { get; set; } =default;

        public DateTime AfspraakDatum { get; set; } = default;

        [Required]
        public int PathLocation { get; set; } = default;

        public int OuderVoogdId { get; set; }
        [ForeignKey(nameof(OuderVoogdId))]
        public OuderVoogd? OuderVoogd { get; set; }

        public int TrajectId { get; set; }
        [ForeignKey(nameof(TrajectId))]
        public Traject? Traject { get; set; }

        public int? ArtsId { get; set; }
        [ForeignKey(nameof(ArtsId))]
        public Arts? Arts { get; set; }

        public ICollection<LogBook>? LogBooks { get; set; } 
    }
}
