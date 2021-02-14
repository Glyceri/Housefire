using NAudio.Wave;
using System;
using System.Threading;

namespace GXPEngine.Core.Audio
{
    public class AudioThread
    {
        public static void PlayAudio(object audioObject)
        {
            try
            {
                (audioObject as AudioObject).audioPlayer?.Play();
                (audioObject as AudioObject).audioFile.Volume = AudioHandler.volume / (float)100;
                while ((audioObject as AudioObject).audioPlayer?.PlaybackState != PlaybackState.Stopped && !(audioObject as AudioObject).disposed)
                {
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            (audioObject as AudioObject).Dispose();
        }
    }
}