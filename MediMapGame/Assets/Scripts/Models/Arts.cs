using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Unity.VisualScripting.Dependencies.Sqlite;

namespace Assets.Scripts.Models
{
    public class Arts
    {
        public int Id { get; set; }
        public string Naam { get; set; } = string.Empty;
        public string Specialisatie { get; set; } = string.Empty;
    }
}
