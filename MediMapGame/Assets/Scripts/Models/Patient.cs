using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Patient
    {
        public int Id;
        public string VoorNaam;
        public string AvatarNaam;
        //public ICollection<LogBook>? logbook;
        public string AchterNaam;
        public string ArtsNaam;
        public string TrajectNaam;
        public string OuderVoogdNaam;
        public DateTime GeboorteDatum;
        public DateTime Afspraakatum;
    }

}
