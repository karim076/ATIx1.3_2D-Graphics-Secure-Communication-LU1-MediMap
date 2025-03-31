using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Dto
{
    public  class ArtsDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Naam { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Specialisatie { get; set; } = string.Empty;
    }
}
