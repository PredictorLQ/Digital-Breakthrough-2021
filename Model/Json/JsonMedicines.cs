using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WebApiMedMyStem
{
    public class JsonMedicines
    {
        [JsonPropertyName("code")]
        public int Code { get; set; } = 200;
        [JsonPropertyName("lastname")]
        public string Text { get; set; }
        public JsonMedicines(string? data)
        {
            Text = data;
            JsonMedicinesFind = CrossingMedical.CrossingItems(data).Select(u=>new JsonMedicinesFind(u)).ToList();
        }
        [JsonPropertyName("items")]
        public List<JsonMedicinesFind> JsonMedicinesFind { get; set; }
    }
    public class JsonMedicinesFind
    {
        [JsonPropertyName("commercial")]
        public string? Commercial { get; set; }
        [JsonPropertyName("standart")]
        public string? Standart { get; set; }
        [JsonPropertyName("findbycommercial")]
        public bool BoolCommercial { get; set; }
        public JsonMedicinesFind(FindMedical FindMedical)
        {
            Commercial = FindMedical.medicament.Commercial;
            Standart = FindMedical.medicament.Standart;
            BoolCommercial = FindMedical.Commercial;
        }
    }
}
