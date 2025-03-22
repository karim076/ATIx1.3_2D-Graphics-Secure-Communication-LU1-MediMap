using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class TrajectZorgMoment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TrajectID { get; set; }
        [ForeignKey("TrajectID")]
        public Traject? Traject { get; set; }
        public int ZorgMomentID { get; set; }
        [ForeignKey("ZorgMomentID")]
        public ZorgMoment? ZorgMoment { get; set; }
        public int Volgorde { get; set; }
    }
}
