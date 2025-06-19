using System.Text.Json.Serialization;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.GestaoEmail
{
    public record CorpoEmailDto
    {
        [JsonPropertyName("tipoArquivo")]
        public string TipoArquivo { get; init; }

        [JsonPropertyName("nomeArquivo")]
        public string NomeArquivo { get; init; }

        [JsonPropertyName("conteudoBase64")]
        public string ConteudoBase64 { get; init; }
    }
}
