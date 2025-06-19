using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.GestaoEmail;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.GestaoEmail.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.GestaoEmail
{
    public class GestaoEmailService : IGestaoEmailService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private static readonly string _urlGestaoEmail = Environment.GetEnvironmentVariable("Annie:UrlGestaoEmail");
        private static readonly string _idAplicacao = Environment.GetEnvironmentVariable("IdAplicacao");
        private static readonly string _email = Environment.GetEnvironmentVariable("Iguatemi:Email");
        private static readonly string _endpointObterTodos= Environment.GetEnvironmentVariable("Annie:EndpointObterTodos");
        private static readonly string _endpointAdicionarMarcador = Environment.GetEnvironmentVariable("Annie:EndpointAdicionarMarcador");
        private static readonly string _endpointBuscarPorId = Environment.GetEnvironmentVariable("Annie:EndpointBuscarPorId");
        private static readonly string _marcadorEmailLido = Environment.GetEnvironmentVariable("Annie:MarcadorEmailLido");
        private static readonly string _labelEmailLido = Environment.GetEnvironmentVariable("Annie:LabelEmailLido");
        private static readonly string _diasAtras = Environment.GetEnvironmentVariable("Annie:DiasAtras");
        private static readonly string _offset = Environment.GetEnvironmentVariable("Annie:Offset");

        public GestaoEmailService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<DadosEmailsObtidosDto>> ObterEmailsNaoLidos(LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService)
        {
            try 
            {
                logProcessoBuilder.AdicionarAlteraçãoMensagemTrace("Buscando todas os emails não lidos.");
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                var json = JsonSerializer.Serialize(new BuscarTodosEmailsDto
                {
                    DiasAtras = int.Parse(_diasAtras),
                    Email = _email,
                    IdAplicacao = int.Parse(_idAplicacao),
                    IncluirSpamLixeira = true,
                    Marcadores = new List<string>(),
                    Query = "",
                    Assunto = "",
                    Offset = int.Parse(_offset)
                });

                var url = $"{_urlGestaoEmail}{_endpointObterTodos}";
                var httpClient = _httpClientFactory.CreateClient("GestaoEmail");

                var response = await httpClient.PostAsync(url, new StringContent(json, null, "application/json"));
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var mensagemErro = $"Ocorreu uma falha ao buscar e-mails no GestãoEmail.\nStatusCode: {response.StatusCode}\nConteúdo resposta: {responseContent}";
                    throw new Exception(mensagemErro);
                }

                var emailsIguatemiDto = JsonSerializer.Deserialize<List<DadosEmailsObtidosDto>>(responseContent);

                return emailsIguatemiDto.Where(email => !email.Marcadores.Contains(_labelEmailLido)).ToList();
            }
            catch (Exception e)
            {
                var mensagemErro = $"Erro inesperado ao consultar emails não lidos.\n{e.Message}";
                logProcessoBuilder.AdicionarAlteraçãoMensagemFalha(mensagemErro);
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                throw new Exception(mensagemErro);
            }
        }

        public async Task AdicionarMarcadorEmailLidoAsync(string idEmail, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService)
        {
            try 
            {
                logProcessoBuilder.AdicionarAlteraçãoMensagemTrace($"Adicionando marcador de email lido. IdEmail: {idEmail}");
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                var json = JsonSerializer.Serialize(new AdicionarMarcadorDto
                {
                    IdEmail = idEmail,
                    Marcador = _marcadorEmailLido,
                    IdAplicacao = int.Parse(_idAplicacao)
                });

                var url = $"{_urlGestaoEmail}{_endpointAdicionarMarcador}";
                var httpClient = _httpClientFactory.CreateClient("GestaoEmail");

                var result = await httpClient.PostAsync(url, new StringContent(json, null, "application/json"));

                if (!result.IsSuccessStatusCode)
                {
                    var mensagemErro = $"Ocorreu uma falha ao adicionar marcador no e-mail de ID {idEmail} no GestãoEmail.\nStatusCode: {result.StatusCode}\nConteúdo resposta: {await result.Content.ReadAsStringAsync()}";
                    throw new Exception(mensagemErro);
                }
            }
            catch (Exception e)
            {
                var mensagemErro = $"Erro inesperado ao adicionar marcador de email lido. IdEmail: {idEmail}.\n{e.Message}";
                logProcessoBuilder.AdicionarAlteraçãoMensagemTrace(mensagemErro);
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                throw new Exception(mensagemErro);
            }
        }

        public async Task<DadosEmailDto> ObterDadosEmailPorIdAsync(string idEmail, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService)
        {
            var json = JsonSerializer.Serialize(new BuscarEmailDto
            {
                IdAplicacao = int.Parse(_idAplicacao),
                IdEmail = idEmail
            });

            var url = $"{_urlGestaoEmail}{_endpointBuscarPorId}";
            var httpClient = _httpClientFactory.CreateClient("GestaoEmail");
            var response = await httpClient.PostAsync(url, new StringContent(json, null, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var mensagemErro = $"Ocorreu uma falha ao buscar e-mails no GestãoEmail.\nStatusCode: {response.StatusCode}\nConteúdo resposta: {responseContent}";
                throw new Exception(mensagemErro);
            }

            return JsonSerializer.Deserialize<DadosEmailDto>(responseContent);
        }
    }
}
