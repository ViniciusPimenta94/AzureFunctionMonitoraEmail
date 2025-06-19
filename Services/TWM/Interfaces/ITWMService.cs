using System.Collections.Generic;
using System.Threading.Tasks;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Enums;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.TWM.Interfaces
{
    public interface ITWMService
    {
        Task<List<FaturasEmAndamentoDto>> ObterFaturasEmAndamento(LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService);
        Task AtualizarCampoCustomizadoTWMAsync(string identificadorFatura, string nomeCampoCustomizado, string descricaoValorCampoCustomizado, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService);
        Task AtualizarStatusIntegracaoTWMAsync(int idFatura, StatusIntegracaoERP statusIntegracao, string mensagem, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService);

    }
}
