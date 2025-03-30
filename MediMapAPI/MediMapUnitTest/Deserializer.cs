using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediMapUnitTest
{
    public class Deserializer
    {
        public static Error Deserialize(object value)
        {
            var sjson = JsonConvert.SerializeObject(value);
            var json = JsonConvert.DeserializeObject<Error>(sjson);
            return json;
        }
    }
    public class Error
    {
        public string message = string.Empty;
    }
}
