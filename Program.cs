using Azure;
using Azure.AI.Language.QuestionAnswering;
using Azure.AI.TextAnalytics;
using Labb1_NLP_QnA_AzureAI.Application;
using Labb1_NLP_QnA_AzureAI.Configuration;
using Labb1_NLP_QnA_AzureAI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Labb1_NLP_QnA_AzureAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            var serviceConfiguration = new ServiceConfiguration(configuration);
            serviceConfiguration.ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            var app = serviceProvider.GetService<App>();

            if (app != null)
            {
                await app.Run();
            }
        }
    }
}
