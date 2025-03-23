using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class Arts
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key] public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Naam { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Specialisatie { get; set; } = string.Empty;

        public ICollection<Patient>? Patients { get; set; }
    }
}
