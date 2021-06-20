using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{
    public class Parsermkb10
    {
        List<LemmasReplace> LemmasReplaceStage2 { get; set; } = ParserUpdateName.LemmasReplaceStage2;
        public List<DataMkb> DataMkb { get; set; } = new List<DataMkb>();
        public List<DataMkbLemmas> DataMkbLemmas { get; set; } = new List<DataMkbLemmas>();
        public List<Lemmas> Lemmas { get; set; } = new List<Lemmas>();
        public List<DataMkbBig> DataMkbBig { get; set; } = new List<DataMkbBig>();
        public List<DataMkbCode> DataMkbCode { get; set; } = new List<DataMkbCode>();
        public int id { get; set; } = 0;
        public int type { get; set; }
        public void ReadList(List<string> data, int Type = 0)
        {
            type = Type;
            for (int i = 0; i < data.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(data[i]))
                {
                    data[i] = data[i].Replace("\n", " ");
                    DataMkbBig _DataMkbBig = new DataMkbBig
                    {
                        Id = i,
                        LastName = data[i]
                    };
                    List<string> code = ParserMkb.GetCode(data[i]);
                    if (code.Any())
                    {
                        for (int j = 0; j < code.Count; j++) 
                            CreateDataMkbCode(code[j], _DataMkbBig);
                    }
                    List<string> diases = ParserMkb.GetDiases(ParserUpdateName.UpdateNameDiases(data[i]));
                    for (int j = 0; j < diases.Count; j++) CreateDataMkbItem(data[i], diases[j], _DataMkbBig);
                    DataMkbBig.Add(_DataMkbBig);
                }
            }
        }
        public void CreateDataMkbCode(string? text, DataMkbBig DataMkbBig)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > 2)
            {
                DataMkbCode item = DataMkbCode.FirstOrDefault(u => u.Code == text);
                if (item == null)
                {
                    mkb10 mkb10 = Basa.mkb10.FirstOrDefault(u => u.Code == text);
                    if (mkb10 == null) return;
                    item = new DataMkbCode
                    {
                        Code = text,
                        mkb10 = mkb10,
                        DataMkbBig = new List<DataMkbBig> { DataMkbBig }
                    };

                    DataMkbCode.Add(item);
                }
                else item.DataMkbBig.Add(DataMkbBig);
                DataMkbBig.DataMkbCode.Add(item);
            }
        }
        public void CreateDataMkbItem(string line, string text, DataMkbBig DataMkbBig)
        {
            if (text.Length > 2)
            {
                id++;
                DataMkbLemmas lemassitems = DataMkbLemmas.FirstOrDefault(u => u.LastNameUpdate == text);
                DataMkb items = new DataMkb
                {
                    Id = id,
                    DataMkbBigId = DataMkbBig.Id,
                    DataMkbBig = DataMkbBig,
                    LastName = line
                };
                if (lemassitems != null) lemassitems.DataMkb.Add(items);
                else
                {
                    lemassitems = new DataMkbLemmas
                    {
                        LastNameUpdate = text,
                        DataMkb = new List<DataMkb> { items },
                        Lemmas = SelectLemmas(text)
                    };

                    for (int k = 0; k < lemassitems.Lemmas.Count; k++)
                        lemassitems.Lemmas[k].DataMkbLemmas.Add(lemassitems);
                    DataMkbLemmas.Add(lemassitems);
                }
                items.DataMkbLemmas = lemassitems;
                DataMkb.Add(items);
                DataMkbBig.DataMkb.Add(items);
            }
        }
        public List<Lemmas> SelectLemmas(string text, bool serach = true, string last = "")
        {
            List<Lemmas> listLemmas = new List<Lemmas>();
            string[] word = text.Trim().Split(new char[] { ' ', '.', ',' });

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i].Length > 0)
                {
                    Lemmas newLemmas = Lemmas.FirstOrDefault(u => u.LastWord == word[i]);
                    LemmasReplace findreplace = LemmasReplaceStage2.FirstOrDefault(u => u.Item1 == word[i]);
                    if (newLemmas == null)
                    {
                        if (findreplace != null && serach)
                            listLemmas.AddRange(SelectLemmas(findreplace.Item2, false, word[i]));
                        else
                        {
                            if (word[i].Length > 2)
                            {
                                Lemmas nnewLemmas = new Lemmas { LastWord = serach ? word[i].ToLower() : last, Word = serach ? "" : word[i].ToLower(), MyStem = serach };
                                listLemmas.Add(nnewLemmas);
                                Lemmas.Add(nnewLemmas);
                            }
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
