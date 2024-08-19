using Labb1_NLP_QnA_AzureAI.Interfaces;
using Labb1_NLP_QnA_AzureAI.Services;
using System.Text;

namespace Labb1_NLP_QnA_AzureAI.Application
{
    public class App
    {
        private readonly IQuestionAnsweringService _questionAnsweringService;
        private readonly ITranslatorService _translatorService;

        public App(IQuestionAnsweringService questionAnsweringService, ITranslatorService translatorService)
        {
            _questionAnsweringService = questionAnsweringService;
            _translatorService = translatorService;
        }

        public async Task Run()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("Hello and welcome to the Paris 2024 Olympics Q&A.\nPlease enter your question below, enter 'quit' to exit the Q&A.\n");

            while (true)
            {
                Console.Write("Q: ");
                string question = Console.ReadLine();

                if (string.IsNullOrEmpty(question))
                {
                    Console.WriteLine("Sorry, I didn't catch that. Your input was empty. Please enter a valid question.\n");
                    continue;
                }

                if (question.ToLower() == "quit")
                {                    
                    break;
                }

                string language = _questionAnsweringService.DetectLanguage(question);                
                var translatedQuestion = await _translatorService.TranslateTextAsync(question, language);
                string answer;
                 
                if (language != "en")
                {
                    var answerForTranslation = await _questionAnsweringService.GetAnswerFromQnAServiceAsync(translatedQuestion);
                    answer = await _translatorService.TranslateTextAsync(answerForTranslation, "en", language);                    
                }
                else
                {
                    answer = await _questionAnsweringService.GetAnswerFromQnAServiceAsync(translatedQuestion);
                }
                                
                Console.WriteLine($"A: {answer}\n");
            }
            Console.WriteLine("\nThank you for using the Paris 2024 Olympics Q&A. Have a great day!");
        }
    }
}
