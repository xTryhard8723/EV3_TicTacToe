using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Runtime.Versioning;

namespace EV3_TicTacToe.Audio
{
    [SupportedOSPlatform("windows")]
    public class SpeechToText
    {

        private string recognizedText = string.Empty;

        [SupportedOSPlatform("windows")]
        public string getCommand()
        {
            SpeechRecognitionEngine speechRecognizer = new SpeechRecognitionEngine();
            Console.WriteLine("Listening for input!");
            var dictationGrammar = new DictationGrammar();
            speechRecognizer.LoadGrammar(dictationGrammar);

            speechRecognizer.SpeechRecognized += (sender, e) =>
            {
                recognizedText = e.Result.Text;
                Console.WriteLine($"Recognized: {recognizedText}");
            };

            speechRecognizer.SetInputToDefaultAudioDevice();

           
            speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
            Thread.Sleep(5000);
            
            //debug:
            //Console.ReadLine();  

            Console.WriteLine($"Final Recognized Text: {recognizedText}");
            speechRecognizer.Dispose();
            return recognizedText;
        }
            
            
    }
}
