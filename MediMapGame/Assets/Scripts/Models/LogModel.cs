using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.models
{
    [Serializable]
    public class LogModel
    {
        public int id;
        public DateTime? date;
        public string place;
        public string note;
    }
}
