using Sn.KeySound;
using System.Text.Json;

PlatformInvoke.AllocConsole();

var keySounds = new KeySoundOptions
{
    KeyPressSounds =
    {
        new KeySound(Key.A, @"C:\Users\slime\Music\AudioEffect\sounds\camera\camera1.wav"),
        new KeySound(Key.B, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.C, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.D, @"C:\Windows\Media\Windows Notify Calendar.wav"), 
        new KeySound(Key.E, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.F, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.G, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.H, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.I, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.K, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.L, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.O, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.P, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.Q, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.R, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.S, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.U, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.V, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.W, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.X, @"C:\Windows\Media\Windows Notify Calendar.wav"),
        new KeySound(Key.Y, @"C:\Windows\Media\Windows Notify Calendar.wav"),


        new KeySound(Key.J, @"C:\Users\slime\Desktop\temp\sounds\kun\ji.wav"),
        new KeySound(Key.N, @"C:\Users\slime\Desktop\temp\sounds\kun\ni.wav"),
        new KeySound(Key.T, @"C:\Users\slime\Desktop\temp\sounds\kun\tai.wav"),
        new KeySound(Key.M, @"C:\Users\slime\Desktop\temp\sounds\kun\mei.wav"),
    },
    KeyReleaseSounds =
    {
        new KeySound(Key.A, @"C:\Users\slime\Music\AudioEffect\sounds\camera\camera2.wav"),
    }
};

var config = new AppConfig(keySounds);
File.WriteAllText("appconfig.json", JsonSerializer.Serialize(config, JsonHelper.Options));


KeySoundApp app = new KeySoundApp(keySounds);
app.Run();

ConsoleUtils.PressAnyKeyToContinue();
