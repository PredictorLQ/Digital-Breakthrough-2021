using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WebApiMedMyStem
{
    public class JsonSpellerItem
    {
        [JsonPropertyName("word")]
        public string Word { get; set; }
        [JsonPropertyName("s")]
        public List<string> NewWords { get; set; }
    }
}
