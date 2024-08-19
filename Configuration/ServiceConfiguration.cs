using Labb1_NLP_QnA_AzureAI.Application;
using Labb1_NLP_QnA_AzureAI.Interfaces;
using Labb1_NLP_QnA_AzureAI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_NLP_QnA_AzureAI.Configuration
{
    public class ServiceConfiguration
    {
        public IConfiguration Configuration { get; }

        public ServiceConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IQuestionAnsweringService, QuestionAnsweringService>();
            services.AddSingleton<ITranslatorService, TranslatorService>();
            services.AddTransient<App>();
        }
    }
}
