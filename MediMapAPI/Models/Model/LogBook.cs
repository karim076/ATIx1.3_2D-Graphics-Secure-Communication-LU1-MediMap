using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class LogBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Note { get; set; } = string.Empty;

        [Required]
        public string Place { get; set; } = string.Empty;

        [ForeignKey("PatientID")]
        public int PatientID { get; set; }

        //[Required]
        //public Patient? Patient { get; set; }

        [Required]
        public DateTime? Date { get; set; }
    }
}
