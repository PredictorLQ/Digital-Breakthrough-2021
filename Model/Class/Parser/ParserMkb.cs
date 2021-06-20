using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{
    //класс - парсер заболеваний
    public static class ParserMkb
    {
        //парсер кодов, вытаскивание списка кодов из строки
        public static List<string> GetCode(string? value)
        {
            List<string> code = new List<string>();
            if (!string.IsNullOrEmpty(value))
            {
                string latvalue = value;
                value = Regex.Replace(value, @"\d\d(\.|,)\d\d(\.|,)\d\d\d\d", " ").Trim();
                List<(string, string)> replaceWords = new List<(string, string)> { (" / ", "/"), (" - ", " "), (",", "."), (";", ".") };
                List<(string, string)> replaceWords2 = new List<(string, string)> { ("А", "A"), ("В", "B"), ("С", "C"), ("Е", "E"), ("Н", "H"), ("К", "K"), ("М", "M"), ("О", "O"), ("Р", "P"), ("Т", "T"), ("Х", "X") };
                value = Regex.Replace(value, @"\s+", " ").Trim().ToUpper();
                for (int i = 0; i < replaceWords.Count; i++) value = value.Replace(replaceWords[i].Item1, replaceWords[i].Item2);
                for (int i = 0; i < replaceWords2.Count; i++) if (value.Length < 15) value = value.Replace(replaceWords2[i].Item1, replaceWords2[i].Item2);
                value = Regex.Replace(value, @"(I|II|III)\s*\D?ст", "", RegexOptions.IgnoreCase).Trim();
                value = Regex.Replace(value, @"[а-яА-ЯёЁ]", "");
                value = Regex.Replace(Regex.Replace(value, SText.PatternReplaceWordMkbCode, ""), @"\s+", " ").Trim();
                if (value.Length >= 2)
                {
                    string[] words = value.Split(" ");
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (i < words.Length && words[i].Length > 1 && words[i].IndexOf("-") == -1)
                        {
                            if (words[i].IndexOf("/") > 0)
                            {
                                List<string> subword = words[i].Split("/").ToList();
                                if (subword.Any())
                                {
                                    char symp = subword[0][0];
                                    bool control = char.IsLetter(symp);

                                    if (!control && char.IsDigit(symp) && i > 0)
                                    {
                                        symp = words[i - 1][^1];
                                        control = char.IsLetter(symp);
                                    }
                                    if (control)
                                    {
                                        for (int j = 0; j < subword.Count; j++)
                                        {
                                            if (!string.IsNullOrWhiteSpace(subword[j]))
                                            {
                                                try
                                                {
                                                    if (char.IsLetter(subword[j][0]))
                                                    {
                                                        if (subword[j].Count(u => char.IsLetter(u)) > 1)
                                                        {
                                                            List<string> list_GetCodeAdd = GetCodeAdd(subword[j]);
                                                            if (list_GetCodeAdd.Any())
                                                            {
                                                                symp = list_GetCodeAdd[^1][0];
                                                                if (j + 1 < subword.Count && subword[j + 1].Length == 1)
                                                                {
                                                                    list_GetCodeAdd[^1] = list_GetCodeAdd.Last() + "." + subword[j + 1];
                                                                    j++;
                                                                }
                                                                code.AddRange(list_GetCodeAdd);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            symp = subword[j][0];
                                                            if (j + 1 < subword.Count && subword[j + 1].Length == 1 && char.IsDigit(subword[j + 1][0]))
                                                            {
                                                                code.Add(subword[j] + "." + subword[j + 1]);
                                                                j++;
                                                            }
                                                            else code.Add(subword[j]);
                                                        }
                                                    }
                                                    else if (char.IsDigit(subword[j][0]))
                                                    {
                                                        if (subword[j].Any(u => char.IsLetter(u)))
                                                        {
                                                            string text_send = string.Empty;
                                                            if (subword[j].ToList().FindIndex(u => char.IsLetter(u)) > 0)
                                                            {
                                                                if (code.Any()) code[^1] += $"." + subword[j][0];
                                                                text_send = subword[j].Remove(0, 1);
                                                            }
                                                            else text_send = symp + subword[j];
                                                            List<string> list_GetCodeAdd = GetCodeAdd(text_send);
                                                            if (list_GetCodeAdd.Any())
                                                            {
                                                                symp = list_GetCodeAdd[^1][0];
                                                                if (j + 1 < subword.Count && subword[j + 1].Length == 1)
                                                                {
                                                                    list_GetCodeAdd[^1] = list_GetCodeAdd.Last() + "." + subword[j + 1];
                                                                    j++;
                                                                }
                                                                code.AddRange(list_GetCodeAdd);
                                                            }
                                                        }
                                                        else code.Add(symp + subword[j]);
                                                    }
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (char.IsLetter(words[i][0])) code.AddRange(GetCodeAdd(words[i]));
                            else if (char.IsDigit(words[i][0]))
                            {
                                bool hasLetters = words[i].AsEnumerable().Any(ch => char.IsLetter(ch));
                                if (hasLetters)
                                {
                                    int index = 0;
                                    for (int j = 0; j < words[i].Length; j++, index++) if (char.IsLetter(words[i][j])) break;
                                    words[i] = words[i].Substring(index);
                                    code.AddRange(GetCodeAdd(words[i]));

                                }
                                else if (i > 0 && words[i - 1].Length == 1)
                                {
                                    char symp = words[i - 1][^1];
                                    if (char.IsLetter(symp))
                                    {
                                        words[i] = symp + words[i];
                                        code.AddRange(GetCodeAdd(words[i]));
                                    }
                                }
                            }
                        }

                    }
                    for (int i = 0; i < code.Count; i++)
                    {
                        if (char.IsLetter(code[i][0]) && code[i].Any(u => char.IsDigit(u)) && code[i].IndexOf("-") == -1 && code[i].Length >= 2 && code[i].Length < 9)
                        {
                            string[] parth = code[i].Split(".");
                            if (parth[0].Length == 2) { parth[0] = $"{parth[0][0]}0{parth[0][1]}"; }
                            code[i] = string.Join(".", parth).Trim();
                            code[i] = code[i].TrimEnd(new char[] { '.', ';' });
                            if (!(code[i].IndexOf(".") > -1) && code[i].Length > 3)
                                code[i] = code[i].Insert(3, ".");
                            if (code[i].ToCharArray().Count(u => u == '.') > 1)
                            {
                                code.RemoveAt(i); i--;
                            }
                        }
                        else { code.RemoveAt(i); i--; }
                    }
                }
            }
            return code;
        }

        //дополнительный метод для вытаскивания кода из строки
        // на вход подается слово
        // возращает список найденных кодов скрепленных между собой
        static List<string> GetCodeAdd(string? text)
        {
            List<string> code = new List<string>();
            if (!string.IsNullOrWhiteSpace(text))
            {
                string word = text[0].ToString(); // получили первый символ
                int count = 1;
                for (int j = 1; j < text.Length; j++)
                {
                    if (char.IsLetter(text[j]))
                    {
                        if (count >= 2) code.Add(word); // минимум 2 символа для кода
                        count = 1;
                        word = text[j].ToString(); // начианем отсчет сначала 
                    }
                    else if (char.IsPunctuation(text[j]) || char.IsDigit(text[j])) { word += text[j]; count++; } // если знак пунктуации или цифра забираем символ
                }
                if (count >= 2) code.Add(word);// минимум 2 символа для кода
            }
            return code;
        }
        //парсер кодов, вытаскивание списка заболеваний из строки
        public static List<string> GetDiases(string? value)
        {
            List<string> answer = new List<string>();
            if (!string.IsNullOrWhiteSpace(value))
            {
                List<string> diases = new List<string>();
                string lasttext = value;
                value = value.Replace("?", " ");
                value = Regex.Replace(value, "[0-9]*", "");
                value = Regex.Replace(value, @"\bст\b", "", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"\s?(\.+|,+)\s?", ".").Trim();
                value = Regex.Replace(value, @"\s+", " ").Trim();
                string text = "";
                int upperCount = value.Count(u => char.IsUpper(u));
                bool isUpper = (float)upperCount / value.Length >= 0.75;
                bool control = false, fiUpper = false, poiUpper = value.Any(u => u == '.') && value.Any(u => char.IsUpper(u));
                string[] words = value.Split(" ");
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].ToLower() == "острый" || words[i].ToLower() == "хронический" && i + 1 < words.Length) words[i + 1] = words[i + 1].ToLower();
                }
                value = string.Join(" ", words).Replace(" .", ".");
                for (int i = 0; i < value.Length; i++)
                {
                    if (!control && char.IsLetter(value[i]))
                    {
                        fiUpper = char.IsUpper(value[i]);
                        control = true;
                    }
                    if (i < value.Length - 1)
                    {
                        if (char.IsLetter(value[i + 1]) &&
                            ((value[i] == ' ' && !isUpper && char.IsUpper(value[i + 1]) && !Convert.ToInt32(value[i + 1]).Between(44, 123))
                            || (value[i] == '.' && (char.IsUpper(value[i + 1]) == fiUpper || poiUpper))))
                        {
                            text = text.TrimEnd(new char[] { '.', ',', ';' }).Trim();
                            int countLetter = text.Count(u => char.IsLetter(u));
                            float weightLetter = (float)countLetter / text.Length;
                            if (countLetter > 2 && weightLetter >= 0.75)
                            {
                                diases.Add(text); text = "";
                                control = false;
                            }
                            else
                            {
                                text = "";
                                control = false;
                            }
                        }
                        else { text += value[i]; }
                    }
                    else { text += value[i]; }
                }
                text = text.TrimEnd(new char[] { '.', ',', ';' }).Trim();

                int countLetterend = text.Count(u => char.IsLetter(u));
                float weightLetterend = (float)countLetterend / text.Length;

                if (countLetterend > 2 && weightLetterend >= 0.75) diases.Add(text);
                for (int i = 0; i < diases.Count; i++)
                {
                    int countLetter = diases[i].Count(u => char.IsLetter(u)); // подсчитываем к-во букв
                    float weightLetter = (float)countLetter / diases[i].Length; // расчитываем % букв
                    if (countLetter > 3 && weightLetter >= 0.75)
                    {
                        string[] diases_words = Regex.Replace(diases[i].Replace(".", " "), @"\s+", " ").Trim().Split(" ").Where(u => u.Length > 2).ToArray();
                        if (diases_words.Any()) answer.Add(string.Join(" ", diases_words).ToLower());
                    }
                }

            }
            return answer.Distinct().ToList();
        }
    }
}
