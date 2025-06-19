using System.Text.Json.Serialization;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Email;

public record CamposRetornoEmailDto
{
    [JsonPropertyName("Nome do Anexo")]
    public string? NomeAnexo { get; init; }

    [JsonPropertyName("Status")]
    public bool Status { get; init; }

    [JsonPropertyName("Número do Pedido")]
    public long NumeroPedido { get; init; }

    [JsonPropertyName("Número da Folha de Serviço")]
    public long NumeroFolhaServico { get; init; }

    [JsonPropertyName("Número FV60")]
    public long NumeroFV60 { get; init; }

    [JsonPropertyName("Observação RPA")]
    public string? ObservacaoRPA { get; init; }

}
