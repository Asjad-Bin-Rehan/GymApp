
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public static class SlackExceptionMethods
    {
        private static readonly string? _webhookUrl = DI.SlackWebhook;
        private static string AppName="Lockkeyz";

        public static async void AddException(Exception ex)
        {
            // Skip if webhook not configured
            if (string.IsNullOrEmpty(_webhookUrl))
            {
                Console.WriteLine($"[Slack] Webhook not configured. Exception: {ex.Message}");
                return;
            }

            var httpClient = new HttpClient();

            var payload = new
            {
                text = $"Exception occurred at {AppName}: {ex.Message} at {DateTime.UtcNow}\nStack Trace:\n{ex.StackTrace}"
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            await httpClient.PostAsync(_webhookUrl, content);
        }
        public static async void AddException(string message)
        {
            // Skip if webhook not configured
            if (string.IsNullOrEmpty(_webhookUrl))
            {
                Console.WriteLine($"[Slack] Webhook not configured. Message: {message}");
                return;
            }

            var httpClient = new HttpClient();

            var payload = new
            {
                text = $"Exception occurred at {AppName}: {message}"
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
           await httpClient.PostAsync(_webhookUrl, content);
        }
    }
}
