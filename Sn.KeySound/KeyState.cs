using System.Runtime.InteropServices;

namespace Sn.KeySound
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct KeyState
    {
        ushort value;

        public KeyState(ushort value)
        {
            this.value = value;
        }

        public bool IsCurrentDown => (value & 0x8000) != 0;

        public bool IsPreviousDown => (value & 0x0001) != 0;

        public bool IsKeyPressed => IsCurrentDown && !IsPreviousDown;

        public bool IsKeyReleased => !IsCurrentDown && IsPreviousDown;
    }
}
