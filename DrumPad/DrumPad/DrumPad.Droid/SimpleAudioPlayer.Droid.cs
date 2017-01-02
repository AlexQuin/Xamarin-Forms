using SimpleAudio;
using System;
using System.IO;

namespace DrumPad.Droid
{
    class SimpleAudioPlayer : ISimpleAudioPlayer
    {
        Android.Media.MediaPlayer player;

        static int index = 0;

        public bool Load(Stream audioStream)
        {
            //cache to the file system
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"cache{index++}.wav");
            var fileStream = File.Create(path);
            audioStream.CopyTo(fileStream);
            fileStream.Close();

            //load the cached audio into MediaPlayer
            player?.Dispose();
            player = new Android.Media.MediaPlayer();
            player.SetDataSource(path);
            player.Prepare();

            return true;
        }

        public void Play()
        {
            if (player == null)
                return;

            if (player.IsPlaying)
            {
                player.Pause();
                player.SeekTo(0);
            }

            player.Start();
        }

        public void Stop ()
        {
            player?.Stop();
        }

        public void Pause ()
        {
            player?.Pause();
        }

        public void SetVolume (double volume)
        {
            volume = Math.Max(0, volume);
            volume = Math.Min(1, volume);

            player.SetVolume((float)volume, (float)volume);
        }
    }
}