using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DnevnikNastroeniya.Data;
using DnevnikNastroeniya.Models;
using DnevnikNastroeniya.Models.Api;

namespace DnevnikNastroeniya.Services
{
    internal class NeuralForecastService : IForecastStrategy
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "sk-0b3b742bfe334abb89dd2dadb483cf61"; 

        public NeuralForecastService()
        {
            _context = new AppDbContext();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<Forecast> GenerateForecastAsync(int clientId)
        {
            var records = _context.Records
                .Where(r => r.IdClient == clientId)
                .OrderByDescending(r => r.CreationDate)
                .Take(10)
                .ToList();

            if (records.Count == 0)
                throw new Exception("Нет записей для анализа.");

            string prompt = "Проанализируй мои записи и предскажи настроение на завтра. " +
                            "Ответь СТРОГО в формате JSON без лишних символов: " +
                            "{ \"forecast\": \"текст прогноза\", \"confidence\": \"высокая/средняя/низкая\", \"state\": \"краткое состояние\" }.\n\nМои записи:\n";

            foreach (var r in records)
            {
                prompt += $"- Дата: {r.CreationDate:dd.MM.yyyy}, Настроение: {r.Mood_Level}/10, Комментарий: {r.Comment}\n";
            }

            var requestData = new DeepSeekRequest();
            requestData.messages.Add(new ApiMessage { role = "user", content = prompt });

            var jsonRequest = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.deepseek.com/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DeepSeekResponse>(jsonResponse);

            string aiText = result.choices[0].message.content;
            aiText = aiText.Replace("```json", "").Replace("```", "").Trim();

            using JsonDocument doc = JsonDocument.Parse(aiText);
            var root = doc.RootElement;

            var newForecast = new Forecast
            {
                Target_Date = DateTime.Now.AddDays(1),
                Mood_Forecast = root.GetProperty("forecast").GetString(),
                Confidence = root.GetProperty("confidence").GetString(),
                State = root.GetProperty("state").GetString(),
                CreationDate = DateTime.Now,
                IdNote = records.First().Id
            };

            _context.Forecasts.Add(newForecast);
            await _context.SaveChangesAsync();

            return newForecast;
        }
    }
}