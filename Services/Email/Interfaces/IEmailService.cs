using System.Threading.Tasks;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.GestaoEmail.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Iguatemi.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.TWM.Interfaces;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Email.Interfaces
{
    public interface IEmailService
    {
        Task MonitorarEmailAsync(LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService, IArquivoAnexoEmailService arquivoAnexoEmailService, IGestaoEmailService gestaoEmailService, IIguatemiService iguatemiService, ITWMService twmService);
    }
}
