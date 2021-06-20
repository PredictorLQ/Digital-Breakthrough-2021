using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WebApiMedMyStem
{
    public class JsonGetCode
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        /// <summary>
        /// The name of the product
        /// </summary>
        /// <example>Men's basketball shoes</example>
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }
        [JsonPropertyName("search")]
        public List<string> Search { get; set; }
    }
}
