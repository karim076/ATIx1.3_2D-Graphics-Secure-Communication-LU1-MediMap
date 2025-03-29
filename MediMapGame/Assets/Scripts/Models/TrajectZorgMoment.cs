using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class TrajectZorgMoment
    {
        public int TrajectID { get; set; }
        public Traject? Traject { get; set; }
        public int ZorgMomentID { get; set; }
        public ZorgMoment? ZorgMoment { get; set; }
        public int Volgorde { get; set; }
    }
}
