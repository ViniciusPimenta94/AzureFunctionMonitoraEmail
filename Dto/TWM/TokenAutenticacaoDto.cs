﻿using Newtonsoft.Json;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM
{
    public class TokenAutenticacaoDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
