using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.GestaoEmail;
public record DadosEmailsObtidosDto
{
    [JsonPropertyName("idEmail")]
    public string IdEmail { get; init; }

    [JsonPropertyName("marcadores")]
    public List<string> Marcadores { get; init; }
}

