using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{
    public class DataMkbBig
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public List<DataMkb> DataMkb { get; set; } = new List<DataMkb>();
        public List<DataMkbCode> DataMkbCode { get; set; } = new List<DataMkbCode>();
    }
    public class DataMkb
    {
        public int DataMkbBigId { get; set; }
        public DataMkbBig DataMkbBig { get; set; }
        public int Id { get; set; }
        public string LastName { get; set; }
        public DataMkbLemmas DataMkbLemmas { get; set; }
    }
    public class Findmkb10
    {
        public mkb10 mkb10 { get; set; }
        public float Weight { get; set; }
    }
    public class DataMkbLemmas
    {
        public string LastNameUpdate { get; set; }
        public string Name { get; set; }
        public List<DataMkb> DataMkb { get; set; } = new List<DataMkb>();
        public List<Findmkb10> Findmkb10 { get; set; } = new List<Findmkb10>();
        public List<Lemmas> Lemmas { get; set; } = new List<Lemmas>();
    }
    public class DataMkbCode
    {
        public string Code { get; set; }
        public mkb10 mkb10 { get; set; }
        public List<DataMkbBig> DataMkbBig { get; set; } = new List<DataMkbBig>();
    }
}
