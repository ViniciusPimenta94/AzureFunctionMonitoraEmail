using System.Collections.Generic;
using System.Threading.Tasks;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Email;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Email.Interfaces
{
    public interface IArquivoAnexoEmailService
    {
        Task<List<DadosArquivoRetornoEmail>> ObterDadosArquivoRetornoEmailPorBase64ArquivoXlsxAsync(string base64String, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService);
    }
}
