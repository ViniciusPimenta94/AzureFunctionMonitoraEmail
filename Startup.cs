using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Configurations;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Elastic;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Iguatemi.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Iguatemi;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.TWM.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.TWM;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Email.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.Email;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.GestaoEmail.Interfaces;
using Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Services.GestaoEmail;

[assembly: FunctionsStartup(typeof(Guiando.TWM.Integrador.Iguatemi.MonitoraEmail.Startup))]

namespace Guiando.TWM.Integrador.Iguatemi.MonitoraEmail
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IElasticService, ElasticService>();
            builder.Services.AddSingleton<IArquivoAnexoEmailService, ArquivoAnexoEmailService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();
            builder.Services.AddSingleton<IGestaoEmailService, GestaoEmailService>();
            builder.Services.AddSingleton<IIguatemiService, IguatemiService>();
            builder.Services.AddSingleton<ITWMService, TWMService>();
            builder.Services.AddElasticSearchConfiguration();
            builder.Services.AddHttpClientConfiguration();
        }
    }
}
