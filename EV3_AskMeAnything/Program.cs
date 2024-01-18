using EV3_AskMeAnything.MainFunction;

public class Program
{
    private static void Main(string[] args)
    {
        var speech = new SpeechRecognition();
        speech.speechToText();
    }
}