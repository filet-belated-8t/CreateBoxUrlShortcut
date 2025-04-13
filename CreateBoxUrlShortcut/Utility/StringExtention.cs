using System;
using System.Runtime.InteropServices;
using System.Text;

namespace filet_belated_8t.Tools.CreateBoxUrlShortcut.Utility
{
    public static class StringExtention
    {
        public static IntPtr ToIntPtr(this string text)
        {
            return ToIntPtr(text, Encoding.UTF8);
        }

        public static IntPtr ToIntPtr(this string text, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(text);

            IntPtr ptr = Marshal.AllocHGlobal(buffer.Length);
            try
            {
                byte[] nulls = new byte[buffer.Length];
                Marshal.Copy(nulls, 0, ptr, buffer.Length);

                Marshal.Copy(buffer, 0, ptr, buffer.Length);
            }
            catch
            {
                Marshal.FreeHGlobal(ptr);
                ptr = IntPtr.Zero;
            }

            return ptr;
        }
    }
}
