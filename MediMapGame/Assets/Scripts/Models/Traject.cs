using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Traject
    {
        public int Id { get; set; }

        public string Naam { get; set; } = string.Empty;

        public ICollection<TrajectZorgMoment>? TrajectZorgMomenten { get; set; }
    }
}
