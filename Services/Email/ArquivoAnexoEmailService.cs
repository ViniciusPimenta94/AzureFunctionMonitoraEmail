using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using OfficeOpenXml;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.Email;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Email.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Email
{
    public class ArquivoAnexoEmailService : IArquivoAnexoEmailService
    {
        private const string tipoPedidoExcecao = "Excecao";
        private const string tipoPedidoRegularizacao = "Regularizacao";
        private const string tipoPedidoContrato = "Contrato";
        private const string tipoPedidoPropostaComercial = "Proposta Comercial";
        private const string tipoFolhaServico = "Folha de Servico";
        private const string tipoFV60 = "FV60";
        private const string numeroPedido = "Número do Pedido";
        private const string numeroFolhaServico = "Número da Folha de Serviço";
        private const string numeroFV60 = "Número FV60";
        private const string celulaXlsxInicial = "A2";
        private const int linhaXlsxInicial = 2;
        private const int colunaXlsxInicial = 1;
        private static List<string> AbasPlanilhaValidacao => new List<string>
        {
            tipoPedidoExcecao,
            tipoPedidoRegularizacao,
            tipoPedidoContrato,
            tipoPedidoPropostaComercial,
            tipoFolhaServico,
            tipoFV60
        };

        public async Task<List<DadosArquivoRetornoEmail>> ObterDadosArquivoRetornoEmailPorBase64ArquivoXlsxAsync(string base64String, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService)
        {
            try
            {
                logProcessoBuilder.AdicionarAlteraçãoMensagemTrace("Interpretando dados do arquivo em anexo do email Iguatemi");
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                var excelPackage = ObterExcelPackage(base64String);
                var dadosArquivosEmailRetorno = new List<DadosArquivoRetornoEmail>();

                foreach (var planilha in AbasPlanilhaValidacao)
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[planilha];

                    if (worksheet.Cells[celulaXlsxInicial].Value == null)
                        continue;

                    var valoresLinhaColunaPlanilha = ObterValoresLinhaColuna(worksheet);

                    dadosArquivosEmailRetorno.Add(new DadosArquivoRetornoEmail
                    {
                        AbaPlanilha = worksheet.Name,
                        CamposEmailDto = ObterCamposEmailRetornoDto(valoresLinhaColunaPlanilha)
                    });
                }

                return dadosArquivosEmailRetorno;
            }
            catch (Exception e)
            {
                var mensagemErro = $"Erro ao interpretar dados do arquivo XLS.\n{e.Message}";
                logProcessoBuilder.AdicionarAlteraçãoMensagemFalha(mensagemErro);
                await elasticService.InserirLogProcessoIntegracaoAsync(logProcessoBuilder.Build());

                throw new Exception(mensagemErro);
            }
        }

        private static ExcelPackage ObterExcelPackage(string base64String)
        {
            var bytesArquivoXlsx = Convert.FromBase64String(base64String);
            var stream = new MemoryStream(bytesArquivoXlsx);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            return new ExcelPackage(stream);
        }

        private static List<CamposRetornoEmailDto> ObterCamposEmailRetornoDto(List<Dictionary<string, object>> valoresLinhaColuna)
        {
            var json = JsonSerializer.Serialize(valoresLinhaColuna);
            return JsonSerializer.Deserialize<List<CamposRetornoEmailDto>>(json);
        }

        private static List<Dictionary<string, object>> ObterValoresLinhaColuna(ExcelWorksheet planilhaSelecionada)
        {
            try
            {
                var rowData = new List<Dictionary<string, object>>();

                int rowCount = planilhaSelecionada.Dimension.Rows;
                int colCount = planilhaSelecionada.Dimension.Columns;

                for (int row = linhaXlsxInicial; row <= rowCount; row++)
                {
                    var linha = new Dictionary<string, object>();

                    for (int col = colunaXlsxInicial; col <= colCount; col++)
                    {
                        string header = planilhaSelecionada.Cells[colunaXlsxInicial, col].Value.ToString();
                        object cellValue = planilhaSelecionada.Cells[row, col].Value;

                        if (header == numeroPedido || header ==  numeroFolhaServico || header == numeroFV60)
                            linha.Add(header, cellValue ?? 0);
                        else
                            linha.Add(header, cellValue);
                    }

                    rowData.Add(linha);
                }

                return rowData;
            }
            catch (Exception e)
            {
                var mensagemErro = $"Erro ao obter valores do arquivo xlsx. Error: {e.Message}";
                throw new Exception(mensagemErro);
            }
        }
    }
}
