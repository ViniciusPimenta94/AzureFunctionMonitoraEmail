﻿using System;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Elastic
{
    public class LogProcessoIntegracaoDto
    {
        private const string processoAtual = "Monitoramento do Retorno por email da Iguatemi";

        #region Transação Aplicação
        public string IdTransacaoAplicacao { get; set; }
        public DateTime DataCriacaoTransacaoAplicacao { get; set; }
        #endregion

        public string MensagemTrace { get; set; }
        public string MensagemAlerta { get; set; }
        public string MensagemFalha { get; set; }
        public string EtapaExecucaoAtual { get; set; }

        public string Resultado { get; set; }

        public static LogProcessoIntegracaoBuilder Create() => new LogProcessoIntegracaoBuilder(Guid.NewGuid().ToString(), DateTime.Now, processoAtual);
    }
}
