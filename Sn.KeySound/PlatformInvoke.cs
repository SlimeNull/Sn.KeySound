using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sn.KeySound
{
    internal class PlatformInvoke
    {

        [DllImport("Kernel32.dll", EntryPoint = "AllocConsole",
                   ExactSpelling = true, CharSet = CharSet.None, SetLastError = false)]
        public extern static bool AllocConsole();


        [DllImport("Kernel32.dll", EntryPoint = "FreeConsole",
                   ExactSpelling = true, CharSet = CharSet.None, SetLastError = false)]
        public extern static bool FreeConsole();


        [DllImport("user32.dll", EntryPoint = "GetAsyncKeyState",
                   ExactSpelling = true, CharSet = CharSet.None, SetLastError = false)]
        private extern static ushort _GetAsyncKeyState(Key key);

        public static KeyState GetAsyncKeyState(Key key) =>
            new KeyState(_GetAsyncKeyState(key));
    }
}
