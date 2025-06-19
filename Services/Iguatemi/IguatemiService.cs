using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Email;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Iguatemi.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.TWM.Interfaces;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Iguatemi
{
    public class IguatemiService : IIguatemiService
    {
        private const string tipoPedidoExcecao = "Excecao";
        private const string tipoPedidoRegularizacao = "Regularizacao";
        private const string tipoPedidoContrato = "Contrato";
        private const string tipoPedidoPropostaComercial = "Proposta Comercial";
        private const string tipoFolhaServico = "Folha de Servico";
        private const string tipoFV60 = "FV60";
        private const string numeroPedidoIntegracao = "Numero Pedido Integracao";
        private const string numeroFolhaServicoIntegracao = "Numero Folha de Servico Integracao";
        private const string numeroFV60Integracao = "Numero FV60 Integracao";

        public async Task ValidarRetornoPlanilhaAsync(List<DadosArquivoRetornoEmail> retornoArquivoEmailDto, List<FaturasEmAndamentoDto> faturasEmAndamentoDto, FaturasEmAndamentoDto faturaMonitorada, string base64ArquivoXlsxRetorno, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService, ITWMService twmService)
        {
            var descricaoCampoCustomizado = string.Empty;
            var numeroCampoCustomizado = string.Empty;

            foreach (var dadoEmailDto in retornoArquivoEmailDto)
            {
                try
                {
                    var camposRetorno = dadoEmailDto.CamposEmailDto
                        .Where(x => x.Status)
                        .Select(x => new CamposRetornoEmailDto
                        {
                            NumeroPedido = x.NumeroPedido,
                            NumeroFolhaServico = x.NumeroFolhaServico,
                            NumeroFV60 = x.NumeroFV60,
                            Status = x.Status,
                            NomeAnexo = x.NomeAnexo
                        }).Distinct().ToList();

                    if (!dadoEmailDto.CamposEmailDto.Any(campo => !campo.Status))
                    {
                        foreach (var statusSucesso in camposRetorno)
                        {
                            faturaMonitorada = faturasEmAndamentoDto.Find(fatura => fatura.Id.ToString() == statusSucesso.NomeAnexo);
                            if (faturaMonitorada != null)
                            {
                                switch (dadoEmailDto.AbaPlanilha)
                                {
                                    case tipoPedidoExcecao:
                                    case tipoPedidoRegularizacao:
                                    case tipoPedidoContrato:
                                    case tipoPedidoPropostaComercial:
                                        descricaoCampoCustomizado = numeroPedidoIntegracao;
                                        numeroCampoCustomizado = statusSucesso.NumeroPedido.ToString();
                                        await twmService.AtualizarCampoCustomizadoTWMAsync(faturaMonitorada.IdentificadorFatura, descricaoCampoCustomizado, numeroCampoCustomizado, logProcessoBuilder, elasticService);
                                        break;

                                    case tipoFolhaServico:
                                        descricaoCampoCustomizado = numeroFolhaServicoIntegracao;
                                        numeroCampoCustomizado = statusSucesso.NumeroFolhaServico.ToString();
                                        await twmService.AtualizarCampoCustomizadoTWMAsync(faturaMonitorada.IdentificadorFatura, descricaoCampoCustomizado, numeroCampoCustomizado, logProcessoBuilder, elasticService);
                                        break;

                                    case tipoFV60:
                                        descricaoCampoCustomizado = numeroFV60Integracao;
                                        numeroCampoCustomizado = statusSucesso.NumeroFV60.ToString();
                                        await twmService.AtualizarCampoCustomizadoTWMAsync(faturaMonitorada.IdentificadorFatura, descricaoCampoCustomizado, numeroCampoCustomizado, logProcessoBuilder, elasticService);
                                        break;
                                }
                                await twmService.AtualizarStatusIntegracaoTWMAsync(faturaMonitorada.Id, Enums.StatusIntegracaoERP.AguardandoPagamentoPedido, $"Retorno do email - Campo {descricaoCampoCustomizado} atualizado: Número {numeroCampoCustomizado}", logProcessoBuilder, elasticService);
                            }
                        }
                    }
                    else
                    {
                        int indiceCampoStatusFalse = dadoEmailDto.CamposEmailDto.ToList().FindIndex(campo => !campo.Status);
                        int idFatura = int.Parse(dadoEmailDto.CamposEmailDto[indiceCampoStatusFalse].NomeAnexo);
                        string mensagem = dadoEmailDto.CamposEmailDto[indiceCampoStatusFalse].ObservacaoRPA;

                        faturaMonitorada = faturasEmAndamentoDto.Find(fatura => fatura.Id.ToString() == idFatura.ToString());
                        if (faturaMonitorada != null)
                            await twmService.AtualizarStatusIntegracaoTWMAsync(idFatura, Enums.StatusIntegracaoERP.IntegracaoErro, mensagem, logProcessoBuilder, elasticService);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Erro ao validar retorno planilha Iguatemi: {e.Message}");
                }
            }
        }
    }
}
