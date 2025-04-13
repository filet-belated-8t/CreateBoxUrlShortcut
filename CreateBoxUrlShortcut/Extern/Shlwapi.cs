using System;
using System.Runtime.InteropServices;
using System.Text;

namespace filet_belated_8t.Tools.CreateBoxUrlShortcut.Extern
{
    internal class Shlwapi
    {
        public const uint ASSOCF_NONE = 0;
        public const uint ASSOCSTR_EXECUTABLE = 2;

        [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint AssocQueryString(uint flags, uint str, string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, ref uint pcchOut);
    }
}
