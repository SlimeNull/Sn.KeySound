using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sn.KeySound
{
    internal class AppConfig
    {
        public AppConfig(KeySoundOptions keySounds)
        {
            KeySounds = keySounds;
        }

        public KeySoundOptions KeySounds { get; }
    }
}
