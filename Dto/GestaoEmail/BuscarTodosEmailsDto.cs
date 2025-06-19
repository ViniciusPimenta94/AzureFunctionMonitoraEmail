using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.GestaoEmail;
public class BuscarTodosEmailsDto
{
    [JsonPropertyName("diasAtras")]
    public int DiasAtras { get; init; }

    [JsonPropertyName("email")]
    public string Email { get; init; }

    [JsonPropertyName("idAplicacao")]
    public int IdAplicacao { get; init; }

    [JsonPropertyName("incluirSpamLixeira")]
    public bool IncluirSpamLixeira { get; init; }

    [JsonPropertyName("marcadores")]
    public List<string> Marcadores { get; init; }

    [JsonPropertyName("query")]
    public string Query { get; init; }

    [JsonPropertyName("assunto")]
    public string Assunto { get; init; }

    [JsonPropertyName("offset")]
    public int Offset { get; init; }

    [JsonPropertyName("pagina")]
    public int Pagina { get; init; }
}
