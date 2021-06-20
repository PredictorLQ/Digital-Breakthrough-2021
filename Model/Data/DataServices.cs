using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WebApiMedMyStem
{
    public class DataServices
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public DataServicesLemmas DataServicesToLemmas { get; set; }
    }
    public class FindServices
    {
        public Services Services { get; set; }
        public float Weight { get; set; }
    }
    public class DataServicesLemmas
    {
        public string LastNameUpdate { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<DataServices> DataServices { get; set; } = new List<DataServices>();
        public List<FindServices> FindServices { get; set; } = new List<FindServices>();
        [JsonIgnore]
        public List<Lemmas> Lemmas { get; set; } = new List<Lemmas>();
    }
}
