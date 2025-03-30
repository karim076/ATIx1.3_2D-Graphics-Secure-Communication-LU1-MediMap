using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class ZorgMoment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Naam { get; set; } = string.Empty;

        [MaxLength(256)]
        public string? Url { get; set; }

        public string? Plaatje { get; set; }

        public int? TijdsduurInMin { get; set; }

        public ICollection<TrajectZorgMoment>? TrajectZorgMomenten { get; set; }
    }
}
