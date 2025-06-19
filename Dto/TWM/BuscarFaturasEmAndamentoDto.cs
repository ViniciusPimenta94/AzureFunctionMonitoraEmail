using System.Collections.Generic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM
{
    public class BuscarFaturasEmAndamentoDto
    {
        public List<int> IdsFornecedor { get; set; }
        public List<int> IdsTipoConta { get; set; }
        public List<int> IdsLocalidade { get; set; }
        public string DataUltimoStatusDe { get; set; }
        public string DataUltimoStatusAte { get; set; }
        public string DataEmissaoAte { get; set; }
        public string DataEmissaoDe { get; set; }
        public string Estado { get; set; }
    }
}
