using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Dependencies.Sqlite;

namespace Assets.Scripts.Models
{
    public class ZorgMoment
    {
        public int Id { get; set; }

        public string Naam { get; set; }

        public string Url { get; set; }

        public byte[] Plaatje { get; set; }

        public int TijdsduurInMin { get; set; }

        public ICollection<TrajectZorgMoment> TrajectZorgMomenten { get; set; }
    }
}
