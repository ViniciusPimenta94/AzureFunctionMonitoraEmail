using System.Threading.Tasks;
using Nest;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Elastic;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic
{
    public class ElasticService : IElasticService
    {
        private const string sucesso = "Sucesso";
        private const string falha = "Falha";

        public readonly IElasticClient _elastic;
        public ElasticService(IElasticClient elasticClient)
        {
            _elastic = elasticClient;
        }

        public async Task InserirLogProcessoIntegracaoAsync(LogProcessoIntegracaoDto logProcessoIntegracaoDto)
        {
            var resultado = sucesso;
            if (!string.IsNullOrEmpty(logProcessoIntegracaoDto.MensagemFalha))
                resultado = falha;

            await InserirLogProcessoAsync(logProcessoIntegracaoDto: logProcessoIntegracaoDto, resultado: resultado);
        }

        private async Task InserirLogProcessoAsync(LogProcessoIntegracaoDto logProcessoIntegracaoDto, string resultado)
        {
            logProcessoIntegracaoDto.Resultado = resultado;

            await _elastic.IndexDocumentAsync(logProcessoIntegracaoDto);
        }
    }
}
