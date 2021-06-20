using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiMedMyStem
{
    //класс для уникально списка слов, для работы с mystem, заполняется в различных местах программы
    public class Lemmas
    {
        public string Word { get; set; }
        public string LastWord { get; set; }
        public bool MyStem { get; set; } = true;
        public bool Speller { get; set; } = true;
        public List<DataServicesLemmas> DataServicesLemmas = new List<DataServicesLemmas>();
        public List<DataMkbLemmas> DataMkbLemmas = new List<DataMkbLemmas>();
    }
    //класс для список замен с внешних фалов, аббревитаур, сокращений и т.д. , заполняется в различных местах программы
    public class LemmasReplace
    {
        public string Item1 { get; set; } // искомое слово/подстрока/строка
        public string Item2 { get; set; } // слово/подстрока/строка на которое заменяем
    }
}
