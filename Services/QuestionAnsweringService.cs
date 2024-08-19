using Azure.AI.Language.QuestionAnswering;
using Azure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.AI.TextAnalytics;
using Labb1_NLP_QnA_AzureAI.Interfaces;

namespace Labb1_NLP_QnA_AzureAI.Services
{
    public class QuestionAnsweringService : IQuestionAnsweringService
    {
        private readonly QuestionAnsweringClient _qnaClient;
        private readonly QuestionAnsweringProject _project;
        private readonly TextAnalyticsClient _textAnalyticsClient;

        public QuestionAnsweringService(IConfiguration configuration)
        {            
            var credential = new AzureKeyCredential(configuration["Azure:LanguageService:SubscriptionKey"]);
            var endpoint = new Uri(configuration["Azure:LanguageService:URL"]);
            string projectName = configuration["Azure:LanguageService:ProjectName"];
            string deploymentName = configuration["Azure:LanguageService:DeploymentName"];                    

            _qnaClient = new QuestionAnsweringClient(endpoint, credential);
            _project = new QuestionAnsweringProject(projectName, deploymentName);
            _textAnalyticsClient = new TextAnalyticsClient(endpoint, credential);
        }

        public async Task<string> GetAnswerFromQnAServiceAsync(string question)
        {
            try
            {                                    
                var response = await _qnaClient.GetAnswersAsync(question, _project);
                return response.Value.Answers.Count > 0 ? response.Value.Answers[0].Answer : "Sorry, no answer found.";
            }
            catch (RequestFailedException ex)
            {                
                return $"Azure Service Error: {ex.Status} {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"An error occured: {ex.Message}";
            }
        }

        public string DetectLanguage(string text)
        {
            var detectedLanguage = _textAnalyticsClient.DetectLanguage(text);            
            return detectedLanguage.Value.Iso6391Name;
        }
    }
}
