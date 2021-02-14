using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace GXPEngine.Core.Audio
{
    public class AudioHandler
    {
        
        const int AVAILABLE_SLOTS = 20;

        static AudioFileReader[] audioFileReaders = new AudioFileReader[AVAILABLE_SLOTS];
        static WaveOutEvent[] waveOutEvents = new WaveOutEvent[AVAILABLE_SLOTS];
        static AudioObject[] audioObjects = new AudioObject[AVAILABLE_SLOTS];

        static int _volume = 12;
        public static int volume { get => _volume; set { value = (int)Mathf.Clamp(value, 0, 100);  _volume = value; foreach (AudioFileReader audioFile in audioFileReaders) { if (audioFile != null) audioFile.Volume = (_volume / (float)100); } }
    }

        static AudioHandler()
        {
            for(int i = 0; i < audioFileReaders.Length; i++)
            {
                audioFileReaders[i] = null;
                waveOutEvents[i] = null;
                audioObjects[i] = null;
            }
        }

        public static int RegisterAudio(string audioPath)
        {
            for(int i = 0; i < audioFileReaders.Length; i++)
            {
                if(audioFileReaders[i] == null)
                {
                    try
                    {
                        AudioFileReader fileReader = new AudioFileReader(audioPath);
                        WaveOutEvent outputDevice = new WaveOutEvent();
                        outputDevice.Init(fileReader);
                        audioFileReaders[i] = fileReader;
                        waveOutEvents[i] = outputDevice;
                        string[] splitPath = audioPath.Split('\\');
                        audioObjects[i] = new AudioObject(i, splitPath[splitPath.Length - 1]);
                        //return audioObjects[i];
                        return i;
                    }
                    catch
                    {
                        Console.WriteLine("File not found at: " + audioPath);
                        return -1;
                    }
                }
            }

            Console.WriteLine("Failed to load file at: " + audioPath + " due to there being no empty audio slots left.");
            return -1;
        }


        public static AudioObject GetAudioObject(int identifier)
        {
            try
            {
                return audioObjects[identifier];
            }
            catch
            {
                return null;
            }
        }

        public static AudioFileReader GetAudioFile(int identifier)
        {
            try
            {
                return audioFileReaders[identifier];
            }
            catch
            {
                Console.WriteLine("No audio file exists at this path. Did you dispose of it properly?");
                return null;
            }
        }

        public static WaveOutEvent GetWaveOutEvent(int identifier)
        {
            try
            {
                return waveOutEvents[identifier];
            }
            catch
            {
                Console.WriteLine("No audioPlayer exists at this path. Did you dispose of it properly?");
                return null;
            }
        }

        public static void Dispose()
        {
            for(int i = 0; i < audioFileReaders.Length; i++)
            {
                audioFileReaders[i]?.Dispose();
                audioFileReaders[i] = null;
                waveOutEvents[i]?.Stop();
                waveOutEvents[i]?.Dispose();
                waveOutEvents[i] = null;
                audioObjects[i] = null;
            }
            
            audioFileReaders = new AudioFileReader[0];
            waveOutEvents = new WaveOutEvent[0];
            audioObjects = new AudioObject[0];
        }

        public static void Dispose(int identifier, AudioObject audioObject)
        {
            if(audioFileReaders[identifier] == null)
            {
                Console.WriteLine("This object should already be disposed. Did you clear the reference properly?");
                return;
            }
            if(audioFileReaders[identifier] != audioObject.audioFile)
            {
                Console.WriteLine("The identifier you try to dispose does not match the object you are disposing. Did you clear the reference properly?");
                return;
            }
            try
            {
                if (!audioObject?.disposed ?? true)
                {
                    waveOutEvents[identifier]?.Dispose();
                    waveOutEvents[identifier] = null;
                    audioFileReaders[identifier]?.Dispose();
                    audioFileReaders[identifier] = null;
                    audioObjects[identifier] = null;
                }


            }
            catch
            {
                Console.WriteLine("Disposing failed. This may lead to serious problems. Check if you failed to clear any references");
                return;
            }
        }
    }
}
