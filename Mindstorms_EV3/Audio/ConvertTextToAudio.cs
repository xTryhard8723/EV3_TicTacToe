﻿using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using NAudio.Lame;
using NAudio.Wave;

namespace EV3_TicTacToe.Audio
{
    public class ConvertTextToAudio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private string? finalFileName;
        public void convert(string text, string? filename = "defaultAudio")
        {
            if (text == null)
            {
                return;
            }
            SpeechSynthesizer reader = new SpeechSynthesizer();
            reader.Rate = 1;

            MemoryStream stream = new MemoryStream();
            reader.SetOutputToWaveStream(stream);

            reader.Speak(text);

            finalFileName = $"{Directory.GetCurrentDirectory()}\\Audio\\AudioOutput\\{filename}.mp3";
            convertWavStreamToMp3File(ref stream, finalFileName);
        }

        public string audioFileName()
        {
            return finalFileName;
        }

        private void convertWavStreamToMp3File(ref MemoryStream ms, string savetofilename)
        {
            //rewind to beginning of stream
            ms.Seek(0, SeekOrigin.Begin);

            using (var retMs = new MemoryStream())
            using (var rdr = new WaveFileReader(ms))
            using (var wtr = new LameMP3FileWriter(savetofilename, rdr.WaveFormat, LAMEPreset.VBR_90))
            {
                rdr.CopyTo(wtr);
            }
        }

    }
}