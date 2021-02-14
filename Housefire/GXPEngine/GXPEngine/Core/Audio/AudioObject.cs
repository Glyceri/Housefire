using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GXPEngine.Core.Audio
{
    public class AudioObject : IDisposable
    {
        public int identifier { get; private set; }

        public string audioFileName { get; set; }

        public bool disposed { get; private set; }

        public AudioFileReader audioFile { get { if (disposed) return null; else return AudioHandler.GetAudioFile(identifier); } }

        public WaveOutEvent audioPlayer { get { if (disposed) return null; else return AudioHandler.GetWaveOutEvent(identifier); } }

        public void Dispose()
        {
            if (disposed) return;
            audioPlayer?.Stop();
            AudioHandler.Dispose(identifier, this);
            disposed = true;
        }

        public void Play()
        {
            if (disposed || audioPlayer?.PlaybackState != PlaybackState.Stopped) return;
            Thread t = new Thread(new ParameterizedThreadStart(AudioThread.PlayAudio));
            t.Start(this);
        }

        public void Pause()
        {
            if (disposed || audioPlayer?.PlaybackState != PlaybackState.Playing) return;
            audioPlayer.Pause();
        }

        public void Continue()
        {
            if (disposed || audioPlayer?.PlaybackState != PlaybackState.Paused) return;
            audioPlayer.Play();
        }

        public AudioObject(int identifier)
        {
            this.identifier = identifier;
        }

        public AudioObject(int identifier, string fileName)
        {
            this.identifier = identifier;
            audioFileName = fileName;
        }
    }
}
