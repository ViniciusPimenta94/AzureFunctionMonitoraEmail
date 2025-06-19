using System.Collections.Generic;
using System.Threading.Tasks;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.GestaoEmail;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.GestaoEmail.Interfaces
{
    public interface IGestaoEmailService
    {
        Task<List<DadosEmailsObtidosDto>> ObterEmailsNaoLidos(LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService);

        Task AdicionarMarcadorEmailLidoAsync(string idEmail, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService);

        Task<DadosEmailDto> ObterDadosEmailPorIdAsync(string idEmail, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService);
    }
}
