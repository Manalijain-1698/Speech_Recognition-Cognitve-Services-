using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace SR_CSDemo
{
    class Program
    {
        static readonly string SPEECh_SERVICE_KEY = "e7e7a29e12aa43b490c18797e1505f39";
        static readonly string SPEECh_SERVICE_REGION = "eastus";

        static async Task Main()
        {
            await RecognizeSpeechAsync();
            Console.WriteLine("Press an key to continue");
            Console.ReadLine();
        }

        static async Task RecognizeSpeechAsync()
        {
            var config = SpeechConfig.FromSubscription(SPEECh_SERVICE_KEY, SPEECh_SERVICE_REGION);
            using var recognizer = new SpeechRecognizer(config);
            Console.WriteLine("Say Something");
            var result = await recognizer.RecognizeOnceAsync();
            var reason = GetRecognitionResult(result);
            Console.WriteLine(reason);


            static string GetRecognitionResult(SpeechRecognitionResult result) =>
               result.Reason switch
               {
                   ResultReason.RecognizedSpeech => $"Recognized:\"{result.Text}\"",
                   ResultReason.NoMatch => $"NOMATCH:Speech could not be recognized",
                   ResultReason.Canceled => GetCancellationResultReason(result),
                   _=>$"Unhandled reason:{result.Reason}"


               };
            
        }

        static string GetCancellationResultReason(SpeechRecognitionResult result)
        {
            var cancellation = CancellationDetails.FromResult(result);
            var reason = $"Cancelled:Reason={cancellation.Reason}";
            if (cancellation.Reason == CancellationReason.Error)
            {
                reason += $"cancelled:Error {cancellation.ErrorCode}\n" +
                    $"cancelled: Error{cancellation.ErrorDetails}\n" +
                    $"cancelled:Did you update subscription plan?";
            }
            return reason;

        }
    }
}
