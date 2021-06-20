using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiMedMyStem
{
    //структура таблицы по бд с лекарствам
    public class medicament
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("commercial")]
        public string? Commercial { get; set; } // торговое наименование
        [JsonIgnore]
        public string? CommercialBig { get; set; } // торговое наименование в верхнем региcтре, по нему поиск в web
        [JsonPropertyName("standart")]
        public string? Standart { get; set; } // стандартизированное наименование 
        [JsonIgnore]
        public string? StandartBig { get; set; } // стандартизированное наименование в верхнем региcтре, по нему поиск в web
        [JsonPropertyName("fcommercial")]
        public string? FindCommercial { get; set; }  // торговое наименование, вырезаны и экранированы спец символы и другие символы для поиска по регулярным выражениям
        [JsonPropertyName("fstandart")]
        public string? FindStandart { get; set; }  // стандартизированное наименование, вырезаны и экранированы спец символы и другие символы для поиска по регулярным выражениям
    }
}
