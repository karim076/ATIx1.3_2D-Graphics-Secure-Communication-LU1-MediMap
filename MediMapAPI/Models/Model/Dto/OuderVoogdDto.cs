using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Dto
{
    public class OuderVoogdDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string VoorNaam { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string AchterNaam { get; set; } = string.Empty;
    }
}
