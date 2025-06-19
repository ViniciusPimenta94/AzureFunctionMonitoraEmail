using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.TWM;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Dto.GestaoEmail;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Email.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Iguatemi.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.TWM.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.GestaoEmail.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Builders.Elastic;

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Email;
public class EmailService : IEmailService
{
    private const string mensagemAnexoEmail = "Segue em anexo o relatório de execução do projeto";
    public async Task MonitorarEmailAsync(LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService, IArquivoAnexoEmailService arquivoAnexoEmailService, IGestaoEmailService gestaoEmailService, IIguatemiService iguatemiService, ITWMService twmService)
    {
        var faturasMonitoradasDto = new FaturasEmAndamentoDto();
        var faturasEmAndamentoDto = await twmService.ObterFaturasEmAndamento(logProcessoBuilder, elasticService);
        var emailsIguatemi = await gestaoEmailService.ObterEmailsNaoLidos(logProcessoBuilder, elasticService);

        foreach (var email in emailsIguatemi)
        {
            try
            {
                await ProcessarEmailAsync(email, faturasEmAndamentoDto, faturasMonitoradasDto, logProcessoBuilder, elasticService, arquivoAnexoEmailService, gestaoEmailService, iguatemiService, twmService);
            }
            catch (Exception e) 
            {
                throw new Exception($"Erro ao processar o email: {e.Message}");
            }
        }
    }

    private static async Task ProcessarEmailAsync(DadosEmailsObtidosDto email, List<FaturasEmAndamentoDto> faturasEmAndamentoDto, FaturasEmAndamentoDto faturasMonitoradasDto, LogProcessoIntegracaoBuilder logProcessoBuilder, IElasticService elasticService, IArquivoAnexoEmailService arquivoAnexoEmailService, IGestaoEmailService gestaoEmailService, IIguatemiService iguatemiService, ITWMService twmService)
    {
        try
        {
            var informacoesEmailDto = await gestaoEmailService.ObterDadosEmailPorIdAsync(email.IdEmail, logProcessoBuilder, elasticService);
            var base64HtmlContent = informacoesEmailDto.CorpoEmail.Find(corpo => corpo.TipoArquivo == "text/html")?.ConteudoBase64;

            if (base64HtmlContent != null && ValidarMensagemHtmlRetornoEmail(base64HtmlContent))
            {
                var base64ArquivoXlsx = informacoesEmailDto.CorpoEmail.Find(corpo => corpo.TipoArquivo == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")?.ConteudoBase64;

                if (base64ArquivoXlsx != null)
                {
                    var retornoArquivoEmailDto = await arquivoAnexoEmailService.ObterDadosArquivoRetornoEmailPorBase64ArquivoXlsxAsync(base64ArquivoXlsx, logProcessoBuilder, elasticService);

                    if (retornoArquivoEmailDto.Any())
                        await iguatemiService.ValidarRetornoPlanilhaAsync(retornoArquivoEmailDto, faturasEmAndamentoDto, faturasMonitoradasDto, base64ArquivoXlsx, logProcessoBuilder, elasticService, twmService);
                }
            }

            await gestaoEmailService.AdicionarMarcadorEmailLidoAsync(email.IdEmail, logProcessoBuilder, elasticService);
        }
        catch (Exception e)
        {
            throw new Exception($"Erro ao processar o email: {e.Message}");
        }
    }

    private static bool ValidarMensagemHtmlRetornoEmail(string base64HtmlContent)
    {
        var bytes = Convert.FromBase64String(base64HtmlContent);
        var mensagemHtml = Regex.Replace(Encoding.UTF8.GetString(bytes), @"<[^>]*>", string.Empty);
        return mensagemHtml.Contains(mensagemAnexoEmail);
    }

}