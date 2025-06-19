using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Enums;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.TWM.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Elastic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.TWM
{
    public class TWMService : ITWMService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private static readonly string _prefixo = Environment.GetEnvironmentVariable("Twm:Prefixo");
        private static readonly string _integracoesEmAndamento = Environment.GetEnvironmentVariable("Twm:IntegracoesEmAndamento");
        private static readonly string _alterarCampoCustomizadoFatura = Environment.GetEnvironmentVariable("Twm:AlterarCampoCustomizadoFatura");
        private static readonly string _alterarStatusIntegracaoFatura = Environment.GetEnvironmentVariable("Twm:AlterarStatusIntegracaoFatura");
        private static readonly string _estadoIntegracao = Environment.GetEnvironmentVariable("Twm:EstadoIntegracao");
        private static readonly int _idUsuarioIntegracaoAutomacao = int.Parse(Environment.GetEnvironmentVariable("Twm:IdUsuarioIntegracaoAutomacao"));

        public TWMService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<FaturasEmAndamentoDto>> ObterFaturasEmAndamento(LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService)
        {
            try 
            {
                logProcessoBuilder.AdicionarAlteraçãoMensagemTrace("Buscando todas as faturas em andamento.");
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                var dataAtual = DateTime.Today;
                var data = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(-2);

                var json = JsonSerializer.Serialize(new BuscarFaturasEmAndamentoDto
                {
                    DataUltimoStatusDe = data.ToString("dd/MM/yyyy"),
                    Estado = _estadoIntegracao
                });

                var url = $"{_prefixo}{_integracoesEmAndamento}";
                var responseContent = await EnviarRequisicaoAsync(url, json, logProcessoBuilder, elasticService);

                return JsonSerializer.Deserialize<List<FaturasEmAndamentoDto>>(responseContent);
            }
            catch (Exception e) 
            {
                var mensagemErro = $"Erro inesperado ao consultar faturas no TWM.\n{e.Message}";
                logProcessoBuilder.AdicionarAlteraçãoMensagemFalha(mensagemErro);
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());
                
                throw new Exception(mensagemErro);
            }
        }

        public async Task AtualizarCampoCustomizadoTWMAsync(string identificadorFatura, string nomeCampoCustomizado, string descricaoValorCampoCustomizado, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService)
        {
            try 
            {
                logProcessoBuilder = LogProcessoIntegracaoDto.Create();

                logProcessoBuilder.AdicionarAlteraçãoMensagemTrace($"Atualizando campo {nomeCampoCustomizado} da fatura {identificadorFatura} no TWM.");
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                var fatura = new CampoCustomizadoFaturaApiUpdateDTO
                {
                    NumeroFatura = identificadorFatura,
                    CamposCustomizadosChaveValor = new List<CampoCustomizadoChaveValor>
                {
                    new CampoCustomizadoChaveValor
                    {
                        Chave = nomeCampoCustomizado,
                        Valor = descricaoValorCampoCustomizado
                    }
                }
                };

                var json = JsonSerializer.Serialize(fatura);
                var url = $"{_prefixo}{_alterarCampoCustomizadoFatura}";
                await EnviarRequisicaoPutAsync(url, json, identificadorFatura, logProcessoBuilder, elasticService);
            }
            catch (Exception e) 
            {
                var mensagemErro = $"Ocorreu uma falha ao atualizar campo {nomeCampoCustomizado} da fatura {identificadorFatura} no TWM.\nErro: {e.Message}";
                logProcessoBuilder.AdicionarAlteraçãoMensagemFalha(mensagemErro);
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());
                
                throw new Exception(mensagemErro);
            }
        }

        public async Task AtualizarStatusIntegracaoTWMAsync(int idFatura, StatusIntegracaoERP statusIntegracao, string mensagem, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService)
        {
            try 
            {
                logProcessoBuilder.AdicionarAlteraçãoMensagemTrace($"Atualizando status integração da fatura no TWM. IdFatura {idFatura}.");
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                var historicoIntegracao = new HistoricoIntegracaoRateioFaturaDTO[]
                {
                    new HistoricoIntegracaoRateioFaturaDTO
                    {
                        IdFatura = idFatura,
                        IdUsuarioResponsavelIntegracao = _idUsuarioIntegracaoAutomacao,
                        StatusIntegracao = statusIntegracao,
                        Mensagem = mensagem,
                        DataAlteracaoStatus = DateTime.Now
                    }
                };

                var json = JsonSerializer.Serialize(historicoIntegracao);
                var url = $"{_prefixo}{_alterarStatusIntegracaoFatura}";
                await EnviarRequisicaoAsync(url, json, logProcessoBuilder, elasticService);
            }
            catch (Exception e)
            {
                var mensagemErro = $"Ocorreu uma falha ao atualizar status integraçao no TWM. IdFatura {idFatura}.\nErro: {e.Message}";
                logProcessoBuilder.AdicionarAlteraçãoMensagemFalha(mensagemErro);
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                throw new Exception(mensagemErro);
            }
        }

        private async Task<string> EnviarRequisicaoAsync(string url, string json, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService)
        {
            var httpClient = _httpClientFactory.CreateClient("TWM");
            var response = await httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var mensagemErro = $"Ocorreu uma falha na requisição para {url}.\nStatusCode: {response.StatusCode}\nConteúdo resposta: {responseContent}";
                throw new Exception(mensagemErro);
            }

            return responseContent;
        }

        private async Task EnviarRequisicaoPutAsync(string url, string json, string identificadorFatura, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService)
        {
            var httpClient = _httpClientFactory.CreateClient("TWM");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resultado = await httpClient.PutAsync(url, content);

            if (!resultado.IsSuccessStatusCode)
            {
                var mensagemErro = $"Falha ao atualizar valor do campo customizado da fatura de ID {identificadorFatura}.\nStatusCode: {resultado.StatusCode}\nConteúdo resposta: {await resultado.Content.ReadAsStringAsync()}";
                throw new Exception(mensagemErro);
            }
        }
    }
}