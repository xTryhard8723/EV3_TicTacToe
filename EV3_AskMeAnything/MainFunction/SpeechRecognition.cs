
using System.Threading.Tasks;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Speech.Synthesis.TtsEngine;

namespace EV3_AskMeAnything.MainFunction
{
    public class SpeechRecognition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        SpeechRecognitionEngine speechRecognitionEngine = new SpeechRecognitionEngine();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private SpeechRecognitionEngine createSpeechEngine(string preferredCulture)
        {
            foreach (RecognizerInfo config in SpeechRecognitionEngine.InstalledRecognizers())
            {
                if (config.Culture.ToString() == preferredCulture)
                {
                    speechRecognitionEngine = new SpeechRecognitionEngine(config);
                    break;
                }
            }

            // if the desired culture is not installed, then load default
            if (speechRecognitionEngine == null)
            {
                Console.WriteLine("The desired culture is not installed " +
                    "on this machine, the speech-engine will continue using "
                    + SpeechRecognitionEngine.InstalledRecognizers()[0].Culture.ToString() +
                    " as the default culture.", "Culture " + preferredCulture + " not found!");
                speechRecognitionEngine = new SpeechRecognitionEngine();
            }

            return speechRecognitionEngine;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public string speechToText()
        {
            speechRecognitionEngine = createSpeechEngine("US-us");
            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.LoadGrammarAsync(new DictationGrammar());
            speechRecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>();

            return "test";
        }

        private void speechRecognized(SpeechRecognizedEventArgs e)
        {

        }
        
    }
}
