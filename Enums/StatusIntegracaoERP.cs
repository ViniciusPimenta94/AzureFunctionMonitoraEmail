namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Enums
{
    public enum StatusIntegracaoERP
    {
        IntegracaoErro = 0,
        IntegracaoSucesso = 1,
        Iniciada = 2,
        ProcessamentoDados = 3,
        InicioIntegracao = 4,
        RecebimentoArquivoIntegrador = 5,
        MapeamentoArquivoIntegrador = 6,
        EnvioDadosErp = 7,
        ArquivoGerado = 8,
        InicioDownloadRealizado = 9,
        ViaInterfaceDeIntegracao = 10,
        ViaEdicaoDeFaturas = 11,
        GerarArquivoErro = 12,
        ViaSolicitacaoSuporte = 13,
        AguardandoPagamentoPedido = 14
    }
}
