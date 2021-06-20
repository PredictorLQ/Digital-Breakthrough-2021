using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WebApiMedMyStem
{
    public class JsonMkb
    {
        [JsonPropertyName("code")]
        public int Code { get; set; } = 200;
        public JsonMkb(List<string> data)
        {

            Parsermkb10 Parsermkb10 = new Parsermkb10();
            Parsermkb10.ReadList(data);
            Task.Run(() => WorkLemmas.GetLemmas(Parsermkb10.Lemmas)).Wait();
            CrossingMkb CrossingMkb = new CrossingMkb(Parsermkb10.DataMkbLemmas);
            Task.Run(() => CrossingMkb.CrossingAsync()).Wait();
            JsonMkbItems = Parsermkb10.DataMkbBig.Select(u => new JsonMkbItems(u)).ToList();
        }
        [JsonPropertyName("items")]
        public List<JsonMkbItems> JsonMkbItems { get; set; }
    }
    public class JsonMkbItems
    {
        public JsonMkbItems(DataMkbBig DataMkbBig)
        {
            LastName = DataMkbBig.LastName;
            JsonMkbItemsFindCode = DataMkbBig.DataMkbCode.Select(u => new JsonMkbItemsFindCode(u.mkb10)).ToList();
            for (int i = 0; i < DataMkbBig.DataMkb.Count; i++) JsonMkbItemsFindName.AddRange(DataMkbBig.DataMkb[i].DataMkbLemmas.Findmkb10.Take(MaxElement.MaxServicesInSet).Select(u => new JsonMkbItemsFindName(u.mkb10, u.Weight)).ToList());
        }
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }
        [JsonPropertyName("code")]
        public List<JsonMkbItemsFindCode> JsonMkbItemsFindCode { get; set; } = new List<JsonMkbItemsFindCode>();
        [JsonPropertyName("items")]
        public List<JsonMkbItemsFindName> JsonMkbItemsFindName { get; set; } = new List<JsonMkbItemsFindName>();
    }
    public class JsonMkbItemsFindName
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("weight")]
        public float Weight { get; set; }
        public JsonMkbItemsFindName(mkb10 mkb10, float Weight)
        {
            Code = mkb10.Code;
            Name = mkb10.Name;
            this.Weight = Weight;
        }
    }
    public class JsonMkbItemsFindCode
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public JsonMkbItemsFindCode(mkb10 mkb10)
        {
            Code = mkb10.Code;
            Name = mkb10.Name;
        }
    }
}
