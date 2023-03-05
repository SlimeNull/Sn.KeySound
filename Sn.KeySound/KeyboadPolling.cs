using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sn.KeySound
{
    internal static class KeyboadPolling
    {
        static KeyboadPolling()
        {
            foreach (var key in Enum.GetValues<Key>())
                KeyStates[key] = LastKeyStates[key] = false;
        }

        public static Dictionary<Key, bool> KeyStates { get; } =
            new Dictionary<Key, bool>();

        public static Dictionary<Key, bool> LastKeyStates { get; } =
            new Dictionary<Key, bool>();

        public static void Poll()
        {
            foreach (var key in Enum.GetValues<Key>())
            {
                LastKeyStates[key] = KeyStates[key];
                KeyStates[key] = PlatformInvoke.GetAsyncKeyState(key).IsCurrentDown;
            }
        }

        public static bool IsKeyDown(Key key) => KeyStates[key];
        public static bool IsLastKeyDown(Key key) => LastKeyStates[key];

        public static bool IsKeyPressed(Key key) => IsKeyDown(key) && !IsLastKeyDown(key);
        public static bool IsKeyReleased(Key key) => !IsKeyDown(key) && IsLastKeyDown(key);
    }
}
