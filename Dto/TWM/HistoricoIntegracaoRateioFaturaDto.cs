using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Enums;
using System;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM
{
    public class HistoricoIntegracaoRateioFaturaDTO
    {
        public int IdFatura { get; set; }

        public int IdUsuarioResponsavelIntegracao { get; set; }

        public StatusIntegracaoERP StatusIntegracao { get; set; }

        public string Mensagem { get; set; }

        public DateTime DataAlteracaoStatus { get; set; }
    }
}
