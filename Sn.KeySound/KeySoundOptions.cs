using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sn.KeySound
{
    internal class KeySoundOptions
    {
        public List<KeySound> KeyPressSounds { get; }
            = new List<KeySound>();

        public List<KeySound> KeyReleaseSounds { get; }
            = new List<KeySound>();

        public List<MultiKeySound> TriggerSounds { get; }
            = new List<MultiKeySound>();

        public List<MultiKeySound> HotkeySounds { get; }
            = new List<MultiKeySound>();
    }

    public record KeySound(Key Key, string SoundPath);
    public record MultiKeySound(Key[] Keys, string SoundPath);
}
