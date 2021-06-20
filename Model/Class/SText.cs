using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApiMedMyStem
{
    static class SText
    {
        public static string NameServicesJson = Environment.CurrentDirectory + "/basa/services.json"; //место расположение бд по услугам
        public static string NameMKBJson = Environment.CurrentDirectory + "/basa/mkb10.json"; //место расположение бд по заболениям
        public static string NameMedicalJson = Environment.CurrentDirectory + "/basa/medicament.json"; //место расположение бд по лекарствам 
        public static string NameStandartJson = Environment.CurrentDirectory + "/basa/stndart.json"; //место расположение бд по стандартам
        public static string NameMyStem = Environment.CurrentDirectory + "/mystem.exe"; //место расположение mystem
        public static string NameDictionaryPolimedHtml = Environment.CurrentDirectory + "/load/npoli"; // дирректория, где лежат html файлы полимедики
        public static string[] ReplaceWordGloabal = { "?", "<", ">", "+", ":", "-", ".", "(", ")", "_", "!", ",", "№", "\"", "\'", "/'", " из ", " в ", " не ", " от ", " на ", " при ", " у ", " и ", " без ", " к ", " об ", " при ", " cito " };
        public static string PatternReplaceWordGloabal = string.Join("|", ReplaceWordGloabal.Select(s => Regex.Escape(s))); // паттерн для глобальной вырезки символов из строк
        public static string[] ReplaceWordMkb = { "?", "<", ">", "+", ":", "-", "(", ")", "_", "!", "№", "\"", "\'", "/'", " из ", " в ", " не ", " от ", " на ", " при ", " у ", " и ", " без ", " к ", " об ", " при ", " cito " };
        public static string PatternReplaceWordMkb = string.Join("|", ReplaceWordMkb.Select(s => Regex.Escape(s))); // паттерн для вырезки символов в методах работы с заболеваниями
        public static string[] ReplaceWordMkbCode = { "?", "[", "]", "<", ">", "+", ":", "(", ")", "_", "!", "№", "\"", "\'" };
        public static string PatternReplaceWordMkbCode = string.Join("|", ReplaceWordMkbCode.Select(s => Regex.Escape(s))); // паттерн для вырезки символов в методах работы с заболеваниями (конкретно по коду)
        public static string[] ReplaceWordNameMedical = { "?", "[", "]", "<", ">", "+", ":", "(", ")", "_", "!", "№", "\"", "\'", ".", "," };
        public static string PatternReplaceWordNameMedical = string.Join("|", ReplaceWordNameMedical.Select(s => Regex.Escape(s))); // паттерн для вырезки символов в методах работы с заболеваниями (конкретно по названию заболевания)
    }
}
