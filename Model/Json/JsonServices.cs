using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WebApiMedMyStem
{
    public class JsonServices
    {
        [JsonPropertyName("code")]
        public int Code { get; set; } = 200;
        public JsonServices(List<string> data)
        {

            ParserServices ParserServices = new ParserServices();
            ParserServices.ReadList(data);
            Task.Run(() => WorkLemmas.GetLemmas(ParserServices.Lemmas)).Wait();
            CrossingServices Crossing = new CrossingServices(ParserServices.DataServicesLemmas);
            Task.Run(() => Crossing.CrossingServiceAsync()).Wait();

            JsonServicesItems = ParserServices.DataServices.Select(u => new JsonServicesItems(u)).ToList();
        }
        [JsonPropertyName("items")]
        public List<JsonServicesItems> JsonServicesItems { get; set; }
    }
    public class JsonServicesItems
    {
        public JsonServicesItems(DataServices DataServices)
        {
            LastName = DataServices.LastName;
            JsonServicesItemsFind = DataServices.DataServicesToLemmas.FindServices.Take(MaxElement.MaxServicesInSet).Select(u => new JsonServicesItemsFind(u.Services, u.Weight)).ToList();
        }
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }
        [JsonPropertyName("items")]
        public List<JsonServicesItemsFind> JsonServicesItemsFind { get; set; }
    }
    public class JsonServicesItemsFind
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("weight")]
        public float Weight { get; set; }
        public JsonServicesItemsFind(Services Services, float Weight)
        {
            Code = Services.Code;
            Name = Services.Name;
            this.Weight = Weight;
        }
    }
}
