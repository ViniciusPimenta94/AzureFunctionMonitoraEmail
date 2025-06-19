using System.Collections.Generic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM
{
    public class CampoCustomizadoFaturaApiUpdateDTO
    {
        public string NumeroFatura { get; set; }
        public IEnumerable<CampoCustomizadoChaveValor> CamposCustomizadosChaveValor { get; set; }
    }
}
