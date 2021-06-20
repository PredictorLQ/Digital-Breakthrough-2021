using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{

    class CrossingServices
    {
        List<Services> Services = Basa.Services;
        List<DataServicesLemmas> DataServicesLemmas { get; set; }
        public CrossingServices(List<DataServicesLemmas> DataServicesLemmas)
        {
            this.DataServicesLemmas = DataServicesLemmas;
        }
        public CrossingServices() { }
        public async Task CrossingServiceAsync(List<DataServicesLemmas> dataServicesLemmas)
        {
            for (int z = 0; z < dataServicesLemmas.Count; z++)
            {
                if (dataServicesLemmas[z] != null)
                    await CrossingServiceItemsAsync(dataServicesLemmas[z], z);
            }
        }
        public async Task CrossingServiceAsync()
        {
            for (int z = 0; z < DataServicesLemmas.Count; z++)
            {
                if (DataServicesLemmas[z] != null)
                    await CrossingServiceItemsAsync(DataServicesLemmas[z], z);
            }
        }
        public async Task CrossingServiceItemsAsync(DataServicesLemmas DataServicesLemmas, int k)
        {
            List<Services> serch = new List<Services>();
            serch.AddRange(Services.Where(u => u.Lemas.IndexOf(DataServicesLemmas.LastNameUpdate) >= 0 && !serch.Any(t => t.Id == u.Id)).ToList());
            if (!serch.Any())
            {
                string[] word = DataServicesLemmas.Lemmas.Select(u => u.Word).ToArray();
                serch.AddRange(Services.Where(u => u.ServicesWord.Any(t => word.Contains(t.Word)) && !serch.Any(t => t.Id == u.Id) && DataServicesLemmas.Lemmas.Count > u.LengthLemas50f).ToList());
                if (!serch.Any())
                {
                    serch.AddRange(Services.Where(u => u.ServicesWord.Any(t => word.Contains(t.Word)) && !serch.Any(t => t.Id == u.Id) && DataServicesLemmas.Lemmas.Count > u.LengthLemas50).ToList());
                    if (!serch.Any())
                    {
                        serch.AddRange(Services.Where(u => u.ServicesWord.Any(t => word.Contains(t.Word)) && !serch.Any(t => t.Id == u.Id) && DataServicesLemmas.Lemmas.Count <= u.LengthLemas50).ToList());
                        if (!serch.Any())
                        {
                            string patter = $@"\b({string.Join("|", word)})\b";
                            serch.AddRange(Services.Where(u => Regex.IsMatch(u.Lemas, patter, RegexOptions.IgnoreCase) && !serch.Any(t => t.Id == u.Id) && DataServicesLemmas.Lemmas.Count > u.LengthLemas50f).ToList());
                            if (!serch.Any())
                                serch.AddRange(Services.Where(u => Regex.IsMatch(u.Lemas, patter, RegexOptions.IgnoreCase) && !serch.Any(t => t.Id == u.Id) && DataServicesLemmas.Lemmas.Count <= u.LengthLemas50).ToList());

                        }
                    }
                }
            }
            if (serch.Any())
            {
                serch = serch.OrderByDescending(u => u.LengthLemas100).ToList();
                await Task.Run(() => GetFindServices(DataServicesLemmas, ref serch, k));
            }

        }
        static void GetFindServices(DataServicesLemmas DataServicesLemmas, ref List<Services> serch, int k)
        {
            List<FindServices> FindServices = new List<FindServices>();
            for (int j = 0; j < serch.Count; j++)
            {
                int count = 0, countnes = 0;
                for (int i = 0; i < DataServicesLemmas.Lemmas.Count; i++)
                    if (serch[j].Words.Contains(DataServicesLemmas.Lemmas[i].Word))
                    {
                        count++;
                        if (serch[j].ServicesWord.Any(u => u.Word == DataServicesLemmas.Lemmas[i].Word && u.Necessarily)) countnes++;
                    }

                float Weight = (float)count / serch[j].LengthLemas100;

                FindServices.Add(new FindServices
                {
                    Services = serch[j],
                    Weight = Weight,
                });
                if (Weight > 0.9) break;
            }
            FindServices = FindServices.OrderByDescending(u => u.Weight).Take(5).ToList();
            DataServicesLemmas.FindServices = GetFindServicesAfter(ref FindServices);
        }
        public static List<FindServices> GetFindServicesAfter(ref List<FindServices> FindServices)
        {
            if (FindServices.Any())
            {
                if (FindServices.Max(u => u.Weight) > 0.9) return FindServices.Take(1).ToList();
                else if (FindServices.Max(u => u.Weight) > 0.8) return FindServices.Take(2).ToList();
                else if (FindServices.Max(u => u.Weight) > 0.65) return FindServices.Take(3).ToList();
            }
            return FindServices;
        }
    }
}
