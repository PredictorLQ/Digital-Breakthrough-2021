using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{
    public static class WorlSpeller
    {
        public static async Task GetSpeller(List<Lemmas> Lemmas)
        {
            List<JsonSpellerItem> JsonSpellerItem = await new SpellerWrapper().GetText(Lemmas.Where(u => u.Speller).Select(u => u.LastWord).ToArray());
            for (int i = 0; i < JsonSpellerItem.Count; i++)
            {
                Lemmas lemas = Lemmas.FirstOrDefault(u => u.LastWord == JsonSpellerItem[i].Word);
                if (lemas != null && JsonSpellerItem[i].NewWords.Any())
                {
                    string[] word = JsonSpellerItem[i].NewWords[0].Split(" ");
                    lemas.LastWord = JsonSpellerItem[i].NewWords[0];
                    if (word.Length > 1)
                    {
                        for (int j = 0; j < word.Length; j++)
                        {
                            Lemmas newLemmas = Lemmas.FirstOrDefault(u => u.LastWord == word[i]);
                            if (newLemmas == null)
                            {
                                newLemmas = new Lemmas
                                {
                                    LastWord = word[j],
                                    DataMkbLemmas = lemas.DataMkbLemmas,
                                    DataServicesLemmas = lemas.DataServicesLemmas
                                };
                                for (int k = 0; k < lemas.DataMkbLemmas.Count; k++)
                                    lemas.DataMkbLemmas[k].Lemmas.Add(newLemmas);
                                for (int k = 0; k < lemas.DataServicesLemmas.Count; k++)
                                    lemas.DataServicesLemmas[k].Lemmas.Add(newLemmas);
                                Lemmas.Add(newLemmas);
                            }
                        }
                        Lemmas.Remove(lemas);
                    }
                }
            }
        }
    }
}
