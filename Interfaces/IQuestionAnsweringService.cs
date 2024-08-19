using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_NLP_QnA_AzureAI.Interfaces
{
    public interface IQuestionAnsweringService
    {
        Task<string> GetAnswerFromQnAServiceAsync(string question);
        string DetectLanguage(string text);
    }
}
