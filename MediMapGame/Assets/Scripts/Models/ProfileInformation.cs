using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ProfileInformation
    {
        public int id;
        public string naam;
        public DateTime geboorteDatum;
        public string naamDokter;
        public string behandelPlan;
        public DateTime afspraakDatum;
        public int patientId;
    }
}
