using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApiMedMyStem
{
    public class JsonStandart
    {
        [JsonPropertyName("code")]
        public int Code { get; set; } = 200;
        [JsonPropertyName("lastname")]
        public string Text { get; set; }
        public JsonStandart(int type, string data, int sex = 0, int age = 0)
        {
            Text = data;
            switch (type)
            {
                case 0:
                    JsonStandartItems = Basa.Standart.Where(u => u.CodeMkb10.Contains(data) && ((u.AgeId == age && age != 0) || (age == 0)) && ((u.SexId == sex && sex != 0) || (sex == 0))).Select(u => new JsonStandartItems(u)).ToList();
                    break;
                case 1:
                    JsonStandartItems = Basa.Standart.Where(u => u.StandartServices.Any(u => u.Code == data) && ((u.AgeId == age && age != 0) || (age == 0)) && ((u.SexId == sex && sex != 0) || (sex == 0))).Select(u => new JsonStandartItems(u)).ToList();
                    break;
                case 2:
                    JsonStandartItems = Basa.Standart.Where(u => u.StandartMedicament.Any(u => u.Name == data) && ((u.AgeId == age && age !=0) || (age == 0)) && ((u.SexId == sex && sex != 0) || (sex == 0))).Select(u => new JsonStandartItems(u)).ToList();
                    break;
            }
        }
        [JsonPropertyName("items")]
        public List<JsonStandartItems> JsonStandartItems { get; set; } = new List<JsonStandartItems>();
    }
    public class JsonStandartItems
    {
        public JsonStandartItems(Standart Standart)
        {
            StandartServices = Standart.StandartServices;
            StandartMedicament = Standart.StandartMedicament;
            StandartRecomendation = Standart.StandartRecomendation;
            Name = Standart.Name;
            SexId = Standart.SexId;
            AgeId = Standart.AgeId;
        }

        [JsonPropertyName("age")]
        public int AgeId { get; set; }
        [JsonPropertyName("sex")]
        public int SexId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("services")]
        public List<StandartServices> StandartServices { get; set; } = new List<StandartServices>();
        [JsonPropertyName("medicament")]
        public List<StandartMedicament> StandartMedicament { get; set; } = new List<StandartMedicament>();
        [JsonPropertyName("recommendations")]
        public StandartRecomendation StandartRecomendation { get; set; }
    }
}
