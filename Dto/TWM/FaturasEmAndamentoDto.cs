using System;
using System.Text.Json.Serialization;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM;

public record FaturasEmAndamentoDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("identificadorFatura")]
    public string IdentificadorFatura { get; set; }
    
    [JsonPropertyName("dtEmissao")]
    public DateTime? DtEmissao { get; set; }

    [JsonPropertyName("dtVencimento")]
    public DateTime? DtVencimento { get; set; }

    [JsonPropertyName("totalFatura")]
    public decimal TotalFatura { get; set; }

    [JsonPropertyName("fornecedor")]
    public string Fornecedor { get; set; }

    [JsonPropertyName("nomeResponsavel")]
    public string NomeResponsavel { get; set; }

    [JsonPropertyName("dataAlteracaoStatus")]
    public DateTime DataAlteracaoStatus { get; set; }

    [JsonPropertyName("localidade")]
    public string Localidade { get; set; }
}
