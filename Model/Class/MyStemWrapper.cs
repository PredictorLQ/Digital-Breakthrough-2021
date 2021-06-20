using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiMedMyStem
{
    //класс работа с mystem - технология Яндекса, лучше смотреть у них информацию как он работает
    public class MyStemWrapper
    {
        private string PathToMyStem = SText.NameMyStem;
        public string Parameters { get; set; } = string.Empty;

        public async Task<string> Analysis(string text)
        {
            if (!File.Exists(PathToMyStem))
                throw new FileNotFoundException("Path to MyStem.exe is not valid! Change 'PathToMyStem' properties or move MyStem.exe in appropriate folder.");
            try
            {
                return await GetResults(text);
            }
            catch { }

            return null;
        }

        public string[] GetWords(string text)
        {
            var str = Analysis(text).Result;
            return str.Substring(1, str.Length - 2).Split(new string[1]
            {
                "}{"
            }, StringSplitOptions.None);
        }

        public string GetText(string text)
        {
            return string.Join(" ", GetWords(text));
        }

        private async Task<string> returnText(string text)
        {
            string itog;
            using (var process = CreateProcess())
            {
                var bytes = Encoding.UTF8.GetBytes(text);
                process.StandardInput.BaseStream.Write(bytes, 0, bytes.Length);
                process.StandardInput.BaseStream.Flush();
                process.StandardInput.BaseStream.Close();
                itog = process.StandardOutput.ReadToEnd();
            }
            return itog;
        }
        private async Task<string> GetResults(string text)
        {
            string itog = "";
            var count = text.Length;
            while (count > 4000)
            {
                var end = 3999;
                while (text[end] != ' ')
                    end--;
                itog += await returnText(text.Substring(0, end));
                text = text.Substring(end);
                count -= 4000;
            }
            itog += await returnText(text.Substring(0, text.Length));
            return itog;
        }

        private Process CreateProcess()
        {
            return Process.Start(new ProcessStartInfo()
            {
                FileName = PathToMyStem,
                Arguments = Parameters ?? string.Empty,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                StandardOutputEncoding = Encoding.UTF8
            });
        }
    }
}