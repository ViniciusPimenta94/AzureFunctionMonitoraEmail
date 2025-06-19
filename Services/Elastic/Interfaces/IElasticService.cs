using System.Threading.Tasks;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Elastic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces
{
    public interface IElasticService
    {
        Task InserirLogProcessoIntegracaoAsync(LogProcessoIntegracaoDto logTransacaoAuditoriaDto);
    }
}
