using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Text;
using System;

namespace WebApiMedMyStem
{
    public static class WorkLemmas
    {
        //основной метод для работы с mystem 
        // gggggggg - разделитель для слов 
        // fffffff - замена слова, что его не нужно оправлять в mystem 
        public static async void GetLemmas(List<Lemmas> Lemmas)
        {
            await WorlSpeller.GetSpeller(Lemmas);
            string value = string.Join(" gggggggg ", Lemmas.Select(u => u.MyStem ? u.LastWord : " fffffff ").ToList());

            MyStemWrapper MyStemWrapper = new MyStemWrapper { Parameters = "-ld" }; // непосредственно работа с классом, который запускает процес для mystem

            string[] answer = Regex.Replace(MyStemWrapper.GetText(value).Replace("?", ""), @"\s+", " ").Trim().Split("gggggggg");
            //присваиваем измененнные слова к исходным
            for (int i = 0; i < Lemmas.Count; i++)
            {
                string text = answer[i].Trim();
                if (text != "fffffff") Lemmas[i].Word = text.Trim();
            }
        }
    }
}