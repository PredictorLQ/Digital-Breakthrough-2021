using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{
    public static class CrossingMedical
    {
        public static List<FindMedical> CrossingItems(string? text)
        {
            List<FindMedical> FindMedical = new List<FindMedical>();
            if (!string.IsNullOrEmpty(text) && text.Length > 2)
            {
                for (int i = 0; i < Basa.medicament.Count; i++)
                {
                    if (!FindMedical.Any(u => u.medicament.Standart == Basa.medicament[i].Standart))
                    {
                        if (Regex.Match(text, $"(^|\\s){Basa.medicament[i].FindStandart}(\\s|\\b|$)", RegexOptions.IgnoreCase).Success)
                            FindMedical.Add(new FindMedical
                            {
                                medicament = Basa.medicament[i]
                            });
                        if (Regex.Match(text, $"(^|\\s){Basa.medicament[i].FindCommercial}(\\s|\\b|$)", RegexOptions.IgnoreCase).Success)
                            FindMedical.Add(new FindMedical
                            {
                                medicament = Basa.medicament[i],
                                Commercial = true
                            });
                    }

                }
            }
            return FindMedical;
        }
    }
}
