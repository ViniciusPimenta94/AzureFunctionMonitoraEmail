using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Email.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.GestaoEmail.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Iguatemi.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.TWM.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Elastic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail
{
    public class IntegracaoIguatemi
    {
        private readonly IElasticService _elasticService;
        private readonly IArquivoAnexoEmailService _arquivoAnexoEmailService;
        private readonly IEmailService _emailService;
        private readonly IGestaoEmailService _gestaoEmailService;
        private readonly IIguatemiService _iguatemiService;
        private readonly ITWMService _twmService;

        public IntegracaoIguatemi(IElasticService elasticService, IArquivoAnexoEmailService arquivoAnexoEmailService, IEmailService emailService, IGestaoEmailService gestaoEmailService, IIguatemiService iguatemiService, ITWMService twmService)
        {

            _elasticService = elasticService;
            _arquivoAnexoEmailService = arquivoAnexoEmailService;
            _emailService = emailService;
            _gestaoEmailService = gestaoEmailService;
            _iguatemiService = iguatemiService;
            _twmService = twmService;
        }

        [FunctionName("IntegracaoIguatemi")]
        public async Task Run([TimerTrigger("%IntervaloTempoFuncao%")] TimerInfo myTimer)
        {
            var logProcessoBuilder = LogProcessoIntegracaoDto.Create();

            try 
            {
                logProcessoBuilder.AdicionarAlteraçãoMensagemTrace("Iniciando processo de integração para o cliente Iguatemi.");
                await _elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                await _emailService.MonitorarEmailAsync(logProcessoBuilder, _elasticService, _arquivoAnexoEmailService, _gestaoEmailService, _iguatemiService, _twmService);

                logProcessoBuilder.AdicionarAlteraçãoMensagemTrace("Finalizando processo de integração para o cliente Iguatemi.");
                await _elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());
            }
            catch (Exception e) 
            {
                throw new Exception($"Ocorreu um erro no processo de monitoramento do email na Integração da Iguatemi: {e.Message}");
            }
        }
    }
}
