using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{
    public static class ParserUpdateName
    {
        private static List<LemmasReplace> LemmasReplaceStageBigFirst { get; set; } = (from hs in File.ReadAllLines(Environment.CurrentDirectory + @"\basa\ReplaceStage0.txt")
                                                                                      where hs[0] != '/'
                                                                                      select hs.Split(',') into temp
                                                                                      select new LemmasReplace { Item1 = temp[0], Item2 = temp[1] }).ToList();
        private static List<LemmasReplace> LemmasReplaceStageBigEnd { get; set; } = (from hs in File.ReadAllLines(Environment.CurrentDirectory + @"\basa\ReplaceStage3.txt")
                                                                                    where hs[0] != '/'
                                                                                    select hs.Split(',') into temp
                                                                                    select new LemmasReplace { Item1 = temp[0], Item2 = temp[1] }).ToList();
        private static List<LemmasReplace> LemmasReplaceStageBig { get; set; } = (from hs in File.ReadAllLines(Environment.CurrentDirectory + @"\basa\ReplaceStage1.txt")
                                                                                 where hs[0] != '/'
                                                                                 select hs.Split('@') into temp
                                                                                 select new LemmasReplace { Item1 = temp[0], Item2 = temp[1] }).ToList();
        private static List<LemmasReplace> LemmasReplaceStage1 { get; set; } = (from hs in File.ReadAllLines(Environment.CurrentDirectory + @"\basa\MKBReplaceBefore.txt")
                                                                               where hs[0] != '/'
                                                                               select hs.Split(',') into temp
                                                                               select new LemmasReplace { Item1 = temp[0], Item2 = temp[1] }).ToList();
        public static List<LemmasReplace> LemmasReplaceStage2 { get; set; } = (from hs in File.ReadAllLines(Environment.CurrentDirectory + @"\basa\MKBReplaceMedium.txt")
                                                                               where hs[0] != '/'
                                                                               select hs.Split(',') into temp
                                                                               select new LemmasReplace { Item1 = temp[0], Item2 = temp[1] }).ToList();

        
        public static string UpdateNameDiases(string? text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                string lasttext = text;
               // Console.WriteLine($"\n\nELEM 1=> <{text}>");
                text = Regex.Replace(text, @"(\bот)*\s*\d\d\.\d\d\.\d\d\d\d", " ").Replace("?",".").Trim();
                text = Regex.Replace(text, @"\b(?:[А-Я][а-я]+ [А-Я][а-я]+ [А-Я][а-я]+)|(?:[А-Я]{2,} [А-Я]{2,} [А-Я]{2,})\b", " ").Trim();
                for (int i = 0; i < LemmasReplaceStageBigFirst.Count; i++)
                    text = Regex.Replace(text, $"{LemmasReplaceStageBigFirst[i].Item1}", LemmasReplaceStageBigFirst[i].Item2, RegexOptions.IgnoreCase);
                Console.WriteLine($"\n\nELEM 2=> <{text}>");
                for (int i = 0; i < LemmasReplaceStage1.Count; i++)
                    text = Regex.Replace(text, @$"\b{LemmasReplaceStage1[i].Item1}\b", LemmasReplaceStage1[i].Item2);

                Console.WriteLine($"\n\nELEM 3=> <{text}>");
                for (int i = 0; i < LemmasReplaceStage2.Count; i++)
                    text = Regex.Replace(text, @$"\b{LemmasReplaceStage2[i].Item1}\b", LemmasReplaceStage2[i].Item2, RegexOptions.IgnoreCase);
                Console.WriteLine($"\n\nELEM 4=> <{text}>");
                string lasttext1 = text;
                for (int i = 0; i < LemmasReplaceStageBig.Count; i++)
                    text = Regex.Replace(text, @$"\b{LemmasReplaceStageBig[i].Item1}\b", LemmasReplaceStageBig[i].Item2, RegexOptions.IgnoreCase);
                Console.WriteLine($"\n\nELEM 5=> <{text}>");

                string newtext = Regex.Replace(Regex.Replace(text, SText.PatternReplaceWordMkb, " ").Replace(",", " "), @"\s+", " ").Trim(); ;
                Console.WriteLine($"\n\nELEM 6=> <{text}>");
                for (int i = 0; i < LemmasReplaceStageBigEnd.Count; i++)
                    newtext = Regex.Replace(newtext, $"{LemmasReplaceStageBigEnd[i].Item1}", LemmasReplaceStageBigEnd[i].Item2, RegexOptions.IgnoreCase);
                Console.WriteLine($"\n\nELEM 7=> <{text}>");
                newtext = Regex.Replace(Regex.Replace(Regex.Replace(newtext, SText.PatternReplaceWordMkb, " ").Replace("/", " "), @"(I|II|III)\s*\D?ст", " ", RegexOptions.IgnoreCase).Trim(), @"\s+", " ").Trim();
                if (newtext.Length > 3) return newtext;
            }
            return null;
        }
        public static string UpdateNameMedical(string value)
        {
            return Regex.Replace(Regex.Replace(value, SText.PatternReplaceWordNameMedical, " ").Replace("/", " "), @"\s+", " ").Trim();
        }
    }
}
