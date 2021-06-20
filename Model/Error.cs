using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApiMedMyStem
{
    public class Error
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}
