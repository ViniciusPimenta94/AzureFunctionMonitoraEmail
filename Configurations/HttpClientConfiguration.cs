using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Polly;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Configurations
{
    public static class HttpClientConfiguration
    {
        private static readonly string _scope = Environment.GetEnvironmentVariable("Annie:GuiandoNotificationAzureAdApiScope");
        private static readonly string _tenantId = Environment.GetEnvironmentVariable("Annie:GuiandoNotificationAzureAdTenantId");
        private static readonly string _clientId = Environment.GetEnvironmentVariable("Annie:GuiandoNotificationAzureAdClientId");
        private static readonly string _clientSecret = Environment.GetEnvironmentVariable("Annie:GuiandoNotificationAzureAdClientSecret");
        private static readonly string _instance = Environment.GetEnvironmentVariable("Annie:GuiandoNotificationAzureAdInstance");

        private static readonly string _prefixo = Environment.GetEnvironmentVariable("Twm:Prefixo");
        private static readonly string _urlToken = Environment.GetEnvironmentVariable("Twm:UrlToken");
        private static readonly string _urlUsername = Environment.GetEnvironmentVariable("Twm:Username");
        private static readonly string _urlPassword = Environment.GetEnvironmentVariable("Twm:Password");

        public static void AddHttpClientConfiguration(this IServiceCollection services)
        {
            services.AddHttpClient("GestaoEmail", config =>
            {
                config.Timeout = new TimeSpan(0, 10, 0);
                var token = ObterTokenGestaoEmailAsync(_scope, _clientId, _clientSecret, _instance, _tenantId).Result;
                config.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            })
            .ConfigurePrimaryHttpMessageHandler(() => { return new SocketsHttpHandler { UseCookies = false }; })
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(300 * retryAttempt)));

            services.AddHttpClient("TWM", config =>
            {
                config.Timeout = new TimeSpan(1, 0, 0);
                var url = $"{_prefixo}{_urlToken}";
                var token = ObterTokenTWMAsync(_urlUsername, _urlPassword, url).Result;
                config.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            })
            .ConfigurePrimaryHttpMessageHandler(() => { return new SocketsHttpHandler { UseCookies = false }; })
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(300 * retryAttempt)));
        }

        private static async Task<string> ObterTokenGestaoEmailAsync(string scope, string clientId, string clientSecret, string instance, string tenantId)
        {
            try
            {
                var scopes = new[] { scope };
                var app = ConfidentialClientApplicationBuilder.Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority($"{instance}{tenantId}")
                    .Build();

                var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

                return result.AccessToken;
            }
            catch (Exception e)
            {
                var mensagemErro = $"Falha ao buscar token de notificação: {e.Message}";
                throw new Exception(mensagemErro);
            }
        }

        private static Task<string> ObterTokenTWMAsync(string username, string password, string url)
        {
            IEnumerable<KeyValuePair<string, string>> listKeyValuePair = new[]
            {
                new KeyValuePair<string,string>("grant_type", "password"),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password)
            };

            try 
            {
                using (var client = new HttpClient())
                {
                    var result = client.PostAsync(url, new FormUrlEncodedContent(listKeyValuePair)).Result;
                    return Task.FromResult(result.Content.ReadAsAsync<TokenAutenticacaoDto>().Result.AccessToken);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Erro inesperado ao obter o token do TWM.\n{e.Message}");
            }
        }
    }
}
