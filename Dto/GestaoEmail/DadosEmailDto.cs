using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.GestaoEmail;
public record DadosEmailDto
{
    [JsonPropertyName("idEmail")]
    public string IdEmail { get; init; }

    [JsonPropertyName("conta")]
    public string Conta { get; init; }

    [JsonPropertyName("origem")]
    public string Origem { get; init; }

    [JsonPropertyName("destino")]
    public string Destino { get; init; }

    [JsonPropertyName("data")]
    public string Data { get; init; }

    [JsonPropertyName("assunto")]
    public string Assunto { get; init; }

    [JsonPropertyName("marcadores")]
    public List<string> Marcadores { get; init; }

    [JsonPropertyName("resumo")]
    public string Resumo { get; init; }

    [JsonPropertyName("corpoEmail")]
    public List<CorpoEmailDto> CorpoEmail { get; init; }
}

