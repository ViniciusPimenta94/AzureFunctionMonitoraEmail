using System.Collections.Generic;
using System.Threading.Tasks;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Email;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.TWM.Interfaces;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Iguatemi.Interfaces
{
    public interface IIguatemiService
    {
        Task ValidarRetornoPlanilhaAsync(List<DadosArquivoRetornoEmail> retornoArquivoEmailDto, List<FaturasEmAndamentoDto> faturasEmAndamentoDto, FaturasEmAndamentoDto faturaMonitorada, string base64ArquivoXlsxRetorno, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService, ITWMService twmService);
    }
}
