using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MiM_iVision
{
    // Declaring iImage class
    public class iVision
    {
        const string dllName = "iVision_x64.dll";

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "iGetiMatchVersion")]
        public extern static IntPtr iGetiMatchVersion();

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "iGetiMatchVersionDate")]
        public extern static IntPtr iGetiMatchVersionDate();

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "iGetiFindVersion")]
        public extern static IntPtr iGetiFindVersion();

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "iGetiFindVersionDate")]
        public extern static IntPtr iGetiFindVersionDate();

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "iGetErrorText")]
        public extern static IntPtr iGetErrorText(E_iVision_ERRORS eError);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "iGetKeySerial")]
        public extern static E_iVision_ERRORS iGetKeySerial(ref int Serial);
    }

}