using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sn.KeySound
{
    internal class KeySoundApp
    {
        Task? mainLoop;
        CancellationTokenSource? cancellationTokenSource;

        WasapiOut output;

        Dictionary<KeySound, KeySoundPlayer> keyPressSoundStreams =
                new Dictionary<KeySound, KeySoundPlayer>();

        Dictionary<KeySound, KeySoundPlayer> keyReleaseSoundStreams =
                new Dictionary<KeySound, KeySoundPlayer>();

        Dictionary<MultiKeySound, KeySoundPlayer> triggerSoundStreams =
                new Dictionary<MultiKeySound, KeySoundPlayer>();

        Dictionary<MultiKeySound, KeySoundPlayer> hotkeySoundStreams =
                new Dictionary<MultiKeySound, KeySoundPlayer>();

        record KeySoundPlayer(MemoryStream Stream, RawSourceWaveStream Wave, IWavePlayer Player);

        public KeySoundOptions Options { get; }

        public KeySoundApp(KeySoundOptions options)
        {
            Options = options;

            var playbackDevice =
                new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

            output = new WasapiOut(playbackDevice, AudioClientShareMode.Shared, true, 0);
        }

        private KeySoundPlayer InitSound(string path)
        {
            using MediaFoundationReader reader = 
                new MediaFoundationReader(path);

            MemoryStream storage =
                new MemoryStream();

            reader.CopyTo(storage);
            storage.Seek(0, SeekOrigin.Begin);

            RawSourceWaveStream wave =
                new RawSourceWaveStream(storage, reader.WaveFormat);


            var playbackDevice =
                new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

            var player = new WasapiOut(playbackDevice, AudioClientShareMode.Shared, true, 0);

            player.Init(wave);

            return new KeySoundPlayer(storage, wave, player);
        }

        private KeySoundPlayer InitKeyPressSound(KeySound keySound)
        {
            return keyPressSoundStreams[keySound] = InitSound(keySound.SoundPath);
        }

        private KeySoundPlayer InitKeyReleaseSound(KeySound keySound)
        {
            return keyReleaseSoundStreams[keySound] = InitSound(keySound.SoundPath);
        }

        private KeySoundPlayer InitTriggerSound(MultiKeySound multiKeySound)
        {
            return triggerSoundStreams[multiKeySound] = InitSound(multiKeySound.SoundPath);
        }

        private KeySoundPlayer InitHotkeySound(MultiKeySound multiKeySound)
        {
            return hotkeySoundStreams[multiKeySound] = InitSound(multiKeySound.SoundPath);
        }


        private void InitAll()
        {
            foreach (KeySound keyPressSound in Options.KeyPressSounds)
                if (!keyPressSoundStreams.TryGetValue(keyPressSound, out var soundPlayer))
                    soundPlayer = InitKeyPressSound(keyPressSound);

            foreach (KeySound keyReleaseSound in Options.KeyReleaseSounds)
                if (!keyReleaseSoundStreams.TryGetValue(keyReleaseSound, out var soundPlayer))
                    soundPlayer = InitKeyReleaseSound(keyReleaseSound);

            foreach (MultiKeySound triggerSound in Options.TriggerSounds)
                if (!triggerSoundStreams.TryGetValue(triggerSound, out var soundPlayer))
                    soundPlayer = InitTriggerSound(triggerSound);
             
            foreach (MultiKeySound hotkeySound in Options.HotkeySounds)
                if (!triggerSoundStreams.TryGetValue(hotkeySound, out var soundPlayer))
                    soundPlayer = InitHotkeySound(hotkeySound);
        }


        private void MainLoop()
        {
            if (cancellationTokenSource == null)
                return;

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            List<Key> keyPressHistory =
                new List<Key>();

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                KeyboadPolling.Poll();
                foreach (Key key in Enum.GetValues<Key>())
                {
                    if (KeyboadPolling.IsKeyPressed(key))
                    {
                        Console.WriteLine(key);
                        keyPressHistory.Add(key);
                    }
                    while (keyPressHistory.Count > 50)
                        keyPressHistory.RemoveAt(0);
                }

                foreach (KeySound keyPressSound in Options.KeyPressSounds)
                {
                    if (KeyboadPolling.IsKeyPressed(keyPressSound.Key))
                    {
                        if (!keyPressSoundStreams.TryGetValue(keyPressSound, out var soundPlayer))
                            soundPlayer = InitKeyPressSound(keyPressSound);

                        soundPlayer.Player.Stop();
                        soundPlayer.Wave.Position = 0;
                        soundPlayer.Player.Play();
                    }
                }

                foreach (KeySound keyReleaseSound in Options.KeyReleaseSounds)
                {
                    if (KeyboadPolling.IsKeyReleased(keyReleaseSound.Key))
                    {
                        if (!keyReleaseSoundStreams.TryGetValue(keyReleaseSound, out var soundPlayer))
                            soundPlayer = InitKeyReleaseSound(keyReleaseSound);

                        soundPlayer.Player.Stop();
                        soundPlayer.Wave.Position = 0;
                        soundPlayer.Player.Play();
                    }
                }

                Thread.Sleep(0);
            }
        }

        public Task StartAsync()
        {
            cancellationTokenSource = new CancellationTokenSource();
            mainLoop = Task.Run(MainLoop);

            return Task.CompletedTask;
        }

        public Task WaitForShutdownAsync()
        {
            if (mainLoop == null)
                return Task.CompletedTask;

            return mainLoop;
        }

        public Task StopAsync()
        {
            cancellationTokenSource?.Cancel();
            return WaitForShutdownAsync();
        }

        public async Task RunAsync()
        {
            await StartAsync();
            await WaitForShutdownAsync();
        }


        public void Start() => StartAsync().Wait();
        public void WaitForShutdown() => WaitForShutdownAsync().Wait();
        public void Stop() => StopAsync().Wait();
        public void Run() => RunAsync().Wait();
    }
}
