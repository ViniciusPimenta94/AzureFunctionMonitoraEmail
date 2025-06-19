using System.Collections.Generic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Email
{
    public record DadosArquivoRetornoEmail
    {
        public string AbaPlanilha { get; init; }

        public IList<CamposRetornoEmailDto> CamposEmailDto { get; init; }
    }
}
