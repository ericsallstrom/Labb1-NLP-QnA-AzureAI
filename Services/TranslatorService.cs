using Azure;
using Azure.AI.TextAnalytics;
using Azure.AI.Translation.Document;
using Azure.AI.Translation.Text;
using Labb1_NLP_QnA_AzureAI.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_NLP_QnA_AzureAI.Services
{
    public class TranslatorService : ITranslatorService
    {
        private readonly TextTranslationClient _client;
        private static readonly HttpClient _httpClient = new();
        private readonly string _endpoint;
        private readonly string _key;
        private readonly string _region;

        public TranslatorService(IConfiguration configuration)
        {
            _endpoint = configuration["Azure:TranslatorService:URL"];
            _key = configuration["Azure:TranslatorService:SubscriptionKey"];
            _region = configuration["Azure:TranslatorService:SubscriptionRegion"];

            var credential = new AzureKeyCredential(_key);
            _client = new TextTranslationClient(credential, _endpoint);
        }

        public async Task<string> TranslateTextAsync(string text, string fromLanguage, string toLanguage = "en")
        {
            if (fromLanguage == toLanguage)
            {
                return text;
            }

            string route = $"/translate?api-version=3.0&from={fromLanguage}&to={toLanguage}";
            object[] body = [new { Text = text }];
            var requestBody = JsonConvert.SerializeObject(body);

            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(_endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", _key);
                request.Headers.Add("Ocp-Apim-Subscription-Region", _region);

                HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var translationResult = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    string translatedText = translationResult[0].translations[0].text;
                    return translatedText;
                }
                else
                {
                    return $"Translation failed: {response.StatusCode}";
                }
            }
        }
    }
}
