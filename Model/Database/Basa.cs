using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{
    public static class Basa
    {
        public static List<medicament> medicament { get; set; } = JsonSerializer.Deserialize<List<medicament>>(File.ReadAllText(SText.NameMedicalJson));
        public static List<mkb10> mkb10 { get; set; } = JsonSerializer.Deserialize<List<mkb10>>(File.ReadAllText(SText.NameMKBJson));
        public static List<Services> Services { get; set; } = JsonSerializer.Deserialize<List<Services>>(File.ReadAllText(SText.NameServicesJson));
        public static List<Standart> Standart { get; set; } = JsonSerializer.Deserialize<List<Standart>>(File.ReadAllText(SText.NameStandartJson));
    }
}
