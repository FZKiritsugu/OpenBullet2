using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RuriLib.Models.Hits.HitOutputs
{
    public class TelegramBotHitOutput : IHitOutput
    {
        public string Token { get; set; }
        public long ChatId { get; set; }
        public bool OnlyHits { get; set; }

        public TelegramBotHitOutput(string token, long chatId, bool onlyHits = true)
        {
            Token = token;
            ChatId = chatId;
            OnlyHits = onlyHits;
        }

        public async Task Store(Hit hit)
        {
            if (OnlyHits && hit.Type != "SUCCESS")
            {
                return;
            }

            using var client = new HttpClient();

            var webhook = $"https://api.telegram.org/bot{Token}/sendMessage";

            var obj = new Dictionary<string, object>()
            {
                { "chat_id", ChatId },
                { "text", hit.ToString() }
            };

            await client.PostAsync(webhook,
                new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
        }
    }
}
