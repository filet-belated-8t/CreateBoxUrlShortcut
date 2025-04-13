using System;
using System.Runtime.InteropServices;

namespace filet_belated_8t.Tools.CreateBoxUrlShortcut.Extern
{
    public static class Sqlite
    {
        public const int SQLITE_OK = 0;
        public const int SQLITE_ROW = 100;
        public const int SQLITE_DONE = 101;
        public const int SQLITE_INTEGER = 1;
        public const int SQLITE_FLOAT = 2;
        public const int SQLITE_TEXT = 3;
        public const int SQLITE_BLOB = 4;
        public const int SQLITE_NULL = 5;

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_open")]
        public static extern int sqlite3_open(string filename, out IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_close")]
        public static extern int sqlite3_close(IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_prepare_v2")]
        public static extern int sqlite3_prepare_v2(IntPtr db, IntPtr zSql, int nByte, out IntPtr ppStmpt, IntPtr pzTail);
        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_step")]
        public static extern int sqlite3_step(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_finalize")]
        public static extern int sqlite3_finalize(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_errmsg")]
        public static extern string sqlite3_errmsg(IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_count")]
        public static extern int sqlite3_column_count(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_origin_name")]
        public static extern IntPtr sqlite3_column_origin_name(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_type")]
        public static extern int sqlite3_column_type(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_int")]
        public static extern int sqlite3_column_int(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_text")]
        public static extern IntPtr sqlite3_column_text(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_double")]
        public static extern double sqlite3_column_double(IntPtr stmHandle, int iCol);
    }
}
