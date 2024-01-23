using System.Runtime.Versioning;
using System.Speech.Synthesis;

namespace EV3_TicTacToe.Audio
{
    public class SpeakAudio
    {
        [SupportedOSPlatform("windows")]
        public void speak(string input)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.Rate = 1;
            synth.Speak(input);
        }
    }
}
