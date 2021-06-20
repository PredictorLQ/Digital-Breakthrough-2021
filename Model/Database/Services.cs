using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiMedMyStem
{
    //структура таблицы по бд с услугами 
    public class Services
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }  // уникальный id элемента 
        [JsonPropertyName("code")]
        public string Code { get; set; } // код услуги
        [JsonPropertyName("name")]
        public string Name { get; set; } // наименование услуги
        [JsonIgnore]
        public string NameBig { get; set; }  // наименование услуги в верхнем регистре, используется в web для поиска по наименованию
        [JsonPropertyName("lemmas")]
        public string Lemas { get; set; } // набор лемм наименования в виде строки
        [JsonPropertyName("lemmas50")]
        public int LengthLemas50 { get; set; }   // половина количества слов-лемм, округленная в большую строну, используется при поиске 
        [JsonPropertyName("lemmas50f")]
        public float LengthLemas50f { get; set; } // половина количества слов-лемм, используется при поиске 
        [JsonPropertyName("lemmas100")]
        public int LengthLemas100 { get; set; } // количество слов-лемм, используется при поиске 
        [JsonPropertyName("cnecessarily")]
        [NotMapped]
        public int CountServicesWordNecessarily { get; set; } // количество обязательных слов, используется в поиске
        [NotMapped]
        [JsonPropertyName("cpreferably")]
        public int CountServicesWordPreferably { get; set; }// количество желательных слов, используется в поиске
        [NotMapped]
        [JsonPropertyName("mwords")]
        public string[] Words { get; set; } // набор лемм в виде массива, используется в поиске 
        [JsonIgnore]
        public string? JsonServicesWord { get; set; } // json-строка, используется в web- для верификации
        [JsonIgnore]
        [Column("TUpdate")]
        public bool Update { get; set; } = false; // флаг о верефикации данной записи, используется в web- для верификации
        [NotMapped]
        [JsonPropertyName("words")]
        public List<ServicesWord> ServicesWord { get; set; } = new List<ServicesWord>(); // набор лемм в виде полного списка, используется в поиске
    }
    //стурктура слова-леммы услуги 
    public class ServicesWord
    {
        [JsonPropertyName("word")]
        public string Word { get; set; } // слво-лемма
        [JsonPropertyName("weight")]
        public float Weight { get; set; } // длина слова 
        [JsonPropertyName("preferably")]
        public bool Preferably { get; set; } // фалаг - желательное слово
        [JsonPropertyName("necessarily")]
        public bool Necessarily { get; set; } // фалаг - обязательное слово
    }
    public static class WorkServices
    {
        public static List<string> GetServices(string? text)
        {
            List<string> services = new List<string>();
            if (!string.IsNullOrWhiteSpace(text))
                services.Add(text);
            return services;
        }
    }
}
