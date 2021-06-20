using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{
    public class ParserServices
    {
        public List<DataServices> DataServices { get; set; } = new List<DataServices>();
        public List<DataServicesLemmas> DataServicesLemmas { get; set; } = new List<DataServicesLemmas>();
        public List<Lemmas> Lemmas { get; set; } = new List<Lemmas>();
        List<LemmasReplace> LemmasReplaceBefore { get; set; } = (from hs in File.ReadAllLines(Environment.CurrentDirectory + @"\basa\ServiceReplaceBefore.txt")
                                                                 where hs[0] != '/'
                                                                 select hs.Split(',') into temp
                                                                 select new LemmasReplace { Item1 = temp[0], Item2 = temp[1] }).ToList();
        List<LemmasReplace> LemmasReplaceMedium { get; set; } = (from hs in File.ReadAllLines(Environment.CurrentDirectory + @"\basa\ServiceReplaceMedium.txt")
                                                                 where hs[0] != '/'
                                                                 select hs.Split(',') into temp
                                                                 select new LemmasReplace { Item1 = temp[0], Item2 = temp[1] }).ToList();
        public int id { get; set; } = 0;
        public void ReadList(List<string> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(data[i]))
                {
                    List<string> services = WorkServices.GetServices(UpdateStr(data[i]));
                    for (int j = 0; j < services.Count; j++) CreateDataServicesItem(data[i], services[i]);
                }
            }

        }
        public void CreateDataServicesItem(string line, string text)
        {
            if (text.Length > 2)
            {
                id++;
                DataServicesLemmas lemassitems = DataServicesLemmas.FirstOrDefault(u => u.LastNameUpdate == text);
                DataServices items = new DataServices
                {
                    Id = id,
                    LastName = line
                };
                if (lemassitems != null) lemassitems.DataServices.Add(items);
                else
                {
                    lemassitems = new DataServicesLemmas
                    {
                        LastNameUpdate = text,
                        DataServices = new List<DataServices> { items },
                        Lemmas = SelectLemmas(text)
                    };
                    for(int k=0;k< lemassitems.Lemmas.Count; k++)
                        lemassitems.Lemmas[k].DataServicesLemmas.Add(lemassitems);
                    DataServicesLemmas.Add(lemassitems);
                }
                items.DataServicesToLemmas = lemassitems;
                DataServices.Add(items);
            }
        }
        public string UpdateStr(string text)
        {
            text = text.ToLower();
            for (int i = 0; i < LemmasReplaceBefore.Count; i++)
                text.Replace(LemmasReplaceBefore[i].Item1, LemmasReplaceBefore[i].Item2);
            return Regex.Replace(Regex.Replace(text, SText.PatternReplaceWordGloabal, " ").Replace("/", " "), @"\s+", " ").Trim().ToLower();
        }
        public List<Lemmas> SelectLemmas(string text, bool serach = true, string last = "")
        {
            List<Lemmas> listLemmas = new List<Lemmas>();
            string[] word = text.Split(" ");

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i].Length > 0)
                {
                    Lemmas newLemmas = Lemmas.FirstOrDefault(u => u.LastWord == word[i]);
                    LemmasReplace findreplace = LemmasReplaceMedium.FirstOrDefault(u => u.Item1 == word[i]);
                    if (newLemmas == null)
                    {
                        if (findreplace != null && serach)
                            listLemmas.AddRange(SelectLemmas(findreplace.Item2, false, word[i]));
                        else
                        {
                            Lemmas nnewLemmas = new Lemmas { LastWord = serach ? word[i].ToLower() : last, Word = serach ? "" : word[i].ToLower(), MyStem = serach };
                            listLemmas.Add(nnewLemmas);
                            Lemmas.Add(nnewLemmas);
                        }
                    }
                    else if (findreplace != null && newLemmas.MyStem)
                    {
                        if (serach) listLemmas.AddRange(SelectLemmas(findreplace.Item2, false, word[i]));
                        else listLemmas.Add(newLemmas);
                    }
                    else listLemmas.Add(newLemmas);
                }
            }
            return listLemmas;
        }
    }
}
