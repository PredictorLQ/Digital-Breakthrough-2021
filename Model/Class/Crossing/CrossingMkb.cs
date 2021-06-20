using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{
    class CrossingMkb
    {
        List<mkb10> mkb10 { get; set; } = Basa.mkb10;
        List<DataMkbLemmas> DataMkbLemmas { get; set; }
        public CrossingMkb(List<DataMkbLemmas> DataMkbLemmas)
        {
            this.DataMkbLemmas = DataMkbLemmas;
        }
        public async Task CrossingAsync()
        {
            for (int i = 0; i < DataMkbLemmas.Count; i++)
                if (DataMkbLemmas[i] != null)
                    await CrossingItemsAsync(DataMkbLemmas[i]);
        }
        public async Task CrossingItemsAsync(DataMkbLemmas DataMkbLemmas)
        {
            if (DataMkbLemmas != null && !string.IsNullOrEmpty(DataMkbLemmas.LastNameUpdate))
            {
                if (DataMkbLemmas.LastNameUpdate.Length > 2)
                {
                    List<mkb10> serch = new List<mkb10>();
                    serch.AddRange(mkb10.Where(u => u.Lemas.IndexOf(DataMkbLemmas.LastNameUpdate) >= 0 && !serch.Any(t => t.Id == u.Id) && DataMkbLemmas.Lemmas.Count > u.LengthLemas50f).ToList());
                    if (!serch.Any())
                    {
                        string patter = $@"\b({string.Join("|", DataMkbLemmas.Lemmas.Where(u => u.Word.Length > 0).Select(u => u.Word).ToArray())})\b";
                        serch.AddRange(mkb10.Where(u => Regex.IsMatch(u.Lemas, patter, RegexOptions.IgnoreCase) && !serch.Any(t => t.Id == u.Id) && DataMkbLemmas.Lemmas.Count > u.LengthLemas50f).ToList());
                        if (!serch.Any())
                            serch.AddRange(mkb10.Where(u => Regex.IsMatch(u.Lemas, patter, RegexOptions.IgnoreCase) && !serch.Any(t => t.Id == u.Id) && DataMkbLemmas.Lemmas.Count <= u.LengthLemas50).ToList());

                    }
                    serch = serch.OrderByDescending(u => u.LengthLemas100).ToList();
                    if (serch.Any()) await Task.Run(() => CrossingFind(DataMkbLemmas, ref serch));

                }
            }
        }
        public static void CrossingFind(DataMkbLemmas DataMkbLemmas, ref List<mkb10> serch)
        {

            List<Findmkb10> Findmkb10 = new List<Findmkb10>();
            for (int j = 0; j < serch.Count; j++)
            {
                int count = 0;
                for (int i = 0; i < DataMkbLemmas.Lemmas.Count; i++) if (serch[j].Word.Contains(DataMkbLemmas.Lemmas[i].Word)) count++;

                float Weight = (float)count / serch[j].LengthLemas100;
                if (count > 0)
                {
                    Findmkb10.Add(new Findmkb10
                    {
                        mkb10 = serch[j],
                        Weight = Weight,
                    });
                    if (Weight > 0.9) break;
                }
            }
            Findmkb10 = Findmkb10.OrderByDescending(u => u.Weight).Take(5).ToList();
            DataMkbLemmas.Findmkb10 = GetFindmkb10After(ref Findmkb10);
        }
        public static List<Findmkb10> GetFindmkb10After(ref List<Findmkb10> Findmkb10)
        {
            if (Findmkb10.Any())
            {
                if (Findmkb10.Max(u => u.Weight) > 0.9) return Findmkb10.Take(1).ToList();
                else if (Findmkb10.Max(u => u.Weight) > 0.8) return Findmkb10.Take(2).ToList();
                else if (Findmkb10.Max(u => u.Weight) > 0.65) return Findmkb10.Take(3).ToList();
            }
            return Findmkb10;
        }
    }
}
