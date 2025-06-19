using System.Text.Json.Serialization;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.GestaoEmail;

public record AdicionarMarcadorDto
{
    [JsonPropertyName("idEmail")]
    public string IdEmail { get; init; }

    [JsonPropertyName("marcador")]
    public string Marcador { get; init; }

    [JsonPropertyName("idAplicacao")]
    public int IdAplicacao { get; init; }
}
