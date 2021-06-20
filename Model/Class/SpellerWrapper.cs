using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;

namespace WebApiMedMyStem
{
    //класс работа с speller - технология Яндекса, лучше смотреть у них информацию как он работает
    public class SpellerWrapper
    {
        List<List<string>> text { get; set; } = new List<List<string>>();
        private List<JsonSpellerItem> JsonSpellerItem { get; set; } = new List<JsonSpellerItem>();
        public async Task<List<JsonSpellerItem>> GetText(string[] data)
        {
            int count = 0;
            List<string> text_data = new List<string>();
            for (int i = data.Length - 1; i >= 0; i--)
            {
                count += data[i].Length;
                if (count > 3999)
                {
                    count = 0;
                    text.Add(text_data);
                    text_data = new List<string>();
                    i++;
                }
                else text_data.Add(data[i]);
            }
            if (count > 0) text.Add(text_data);
            Task.WaitAll(text.Select(u => Task.Run(() => GetTextAdd(u))).ToArray());
            return JsonSpellerItem;
        }
        private async Task GetTextAdd(List<string> data)
        {
            WebRequest request = WebRequest.Create("https://speller.yandex.net/services/spellservice.json/checkTexts?text=" + string.Join("&text=", data));
            WebResponse response = await request.GetResponseAsync();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            var anse = reader.ReadToEnd();

            JsonSpellerItem[][] answer = JsonSerializer.Deserialize<JsonSpellerItem[][]>(anse);
            for (int i = 0; i < answer.Length; i++)
                if (answer[i].Any()) JsonSpellerItem.AddRange(answer[i]);
        }
    }
}
