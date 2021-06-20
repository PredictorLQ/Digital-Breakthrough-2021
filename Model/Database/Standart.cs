using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WebApiMedMyStem
{
    public class Standart
    {
        //виды AgeId
        // 0 - любой
        // 1 - дети
        // 2 - взрослые

        [JsonPropertyName("age")]
        public int AgeId { get; set; } // тип возраста пациента для стандарта
        //виды SexId
        // 0 - любой
        // 1 - мужчины
        // 2 - женщины
        [JsonPropertyName("sex")]
        public int SexId { get; set; } // тип пола пациента для стандарта
        [JsonPropertyName("code")]
        public List<string> CodeMkb10 { get; set; } // Код по МКБ X - Нозологическая единица
        [JsonPropertyName("name")]
        public string Name { get; set; } // наименование стандарта 
        [JsonPropertyName("services")]
        public List<StandartServices> StandartServices { get; set; } = new List<StandartServices>(); // оказываемые услуги
        [JsonPropertyName("medicament")]
        public List<StandartMedicament> StandartMedicament { get; set; } = new List<StandartMedicament>(); // назначаемые лекарства
        [JsonPropertyName("recommendations")]
        public StandartRecomendation StandartRecomendation { get; set; }
    }
    public class StandartServices
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } // Код медицинской услуги
        [JsonPropertyName("name")]
        public string Name { get; set; }  // Наименование медицинской услуги
        [JsonPropertyName("weight")]
        public float Weight { get; set; } // Усредненный показатель частоты предоставления
        [JsonPropertyName("count")]
        public int Count { get; set; } // Усредненный показатель кратности применения
    }
    public class StandartMedicament
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } // Код
        [JsonPropertyName("name")]
        public string Name { get; set; } // Наименование лекарственного препарата
        [JsonPropertyName("weight")]
        public float Weight { get; set; } // Усредненный показатель частоты предоставления
    }
    public class StandartRecomendation
    {
        [JsonPropertyName("brief_info")]
        public string BriefInfo { get; set; }
    }
}
