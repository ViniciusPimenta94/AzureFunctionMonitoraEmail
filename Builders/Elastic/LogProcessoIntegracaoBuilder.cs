using System;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Elastic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic
{
    public class LogProcessoIntegracaoBuilder
    {
        private LogProcessoIntegracaoDto _logProcessoIntegracao;

        public LogProcessoIntegracaoBuilder(string idTransacaoAplicacao, DateTime dataCriacaoTransacaoAplicacao, string etapaExecucaoAtual)
            => _logProcessoIntegracao = new LogProcessoIntegracaoDto()
            {
                IdTransacaoAplicacao = idTransacaoAplicacao,
                DataCriacaoTransacaoAplicacao = dataCriacaoTransacaoAplicacao,
                EtapaExecucaoAtual = etapaExecucaoAtual
            };

        public LogProcessoIntegracaoBuilder AdicionarAlteraçãoMensagemTrace(string mensagemTrace)
        {
            _logProcessoIntegracao.MensagemTrace = mensagemTrace;
            return this;
        }

        public LogProcessoIntegracaoBuilder AdicionarAlteraçãoMensagemAlerta(string mensagemAlerta)
        {
            _logProcessoIntegracao.MensagemAlerta = mensagemAlerta;
            return this;
        }

        public LogProcessoIntegracaoBuilder AdicionarAlteraçãoMensagemFalha(string mensagemFalha)
        {
            _logProcessoIntegracao.MensagemFalha = mensagemFalha;
            return this;
        }

        public LogProcessoIntegracaoDto Build() => _logProcessoIntegracao;
    }
}
