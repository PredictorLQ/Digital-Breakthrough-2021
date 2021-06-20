using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiMedMyStem
{
    //структура таблицы по бд с заболеваниями 
    public class mkb10
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; } // уникальный id элемента 
        [JsonPropertyName("code")]
        public string Code { get; set; } // код заболевания
        [JsonPropertyName("name")]
        public string Name { get; set; } // наименование заболевания
        [JsonIgnore]
        public string NameBig { get; set; } // наименование заболенивания в верхнем регистре, используется в web для поиска по наименованию
        [JsonPropertyName("lemas")]
        public string Lemas { get; set; } // набор лемм наименования в виде строки 
        [JsonPropertyName("words")]
        [NotMapped]
        public List<string> Word { get; set; } = new List<string>(); // набор лемм в виде списка, используется в поиске 
        [JsonPropertyName("lengthLemas100")]
        public int LengthLemas100 { get; set; } // количество слов-лемм, используется при поиске 
        [JsonPropertyName("lengthLemas50f")]
        public double LengthLemas50f { get; set; } // половина количества слов-лемм, используется при поиске 
        [JsonPropertyName("lengthLemas50")]
        public int LengthLemas50 { get; set; } // половина количества слов-лемм, округленная в большую строну, используется при поиске 
        [JsonIgnore]
        public bool True { get; set; } // поле для верификации правильности заполнения 
    }
    //структура таблицы по бд с стандартам 
    public class phc_garant_normalized
    {
        public int Id { get; set; } // уникальный id элемента 
        public string Code { get; set; } // код стандарта (код мкб-10)
        public string Name { get; set; } // наименование стандарта
        public string Act { get; set; } // описание стандарта
        public string category { get; set; } // наименование категории
    }
}
