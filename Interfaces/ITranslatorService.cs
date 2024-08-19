using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_NLP_QnA_AzureAI.Interfaces
{
    public interface ITranslatorService
    {
        Task<string> TranslateTextAsync(string text, string fromLanguage, string toLanguage = "en");
    }
}
