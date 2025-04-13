using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using filet_belated_8t.Tools.CreateBoxUrlShortcut.Extern;

namespace filet_belated_8t.Tools.CreateBoxUrlShortcut.Utility
{
    public static class IntPtrExtention
    {
        public static int GetSize(this IntPtr ptr)
        {
            int length = 0;
            while (Marshal.ReadByte(ptr, length) != 0) length++;

            return length;
        }

        public static String ToString(this IntPtr ptr,Encoding encoding)
        {
            int lenght = ptr.GetSize();

            byte[] buffer = new byte[lenght];
            Marshal.Copy(ptr, buffer, 0, lenght);

            return encoding.GetString(buffer);
        }

        public static Dictionary<string, object> RowToDictionaly(this IntPtr ptr)
        { 
            Dictionary<string, object> recode = new Dictionary<string, object>();

            int rowCount = Sqlite.sqlite3_column_count(ptr);
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                string columnName = Sqlite.sqlite3_column_origin_name(ptr, rowIndex).ToString(Encoding.UTF8);

                object value = null;
                switch (Sqlite.sqlite3_column_type(ptr, rowIndex))
                {
                    case Sqlite.SQLITE_INTEGER:
                        value = Sqlite.sqlite3_column_int(ptr, rowIndex);
                        break;
                    case Sqlite.SQLITE_FLOAT:
                        value = Sqlite.sqlite3_column_double(ptr, rowIndex);
                        break;
                    case Sqlite.SQLITE_TEXT:
                        value = Sqlite.sqlite3_column_text(ptr, rowIndex).ToString(Encoding.UTF8);
                        break;
                    case Sqlite.SQLITE_BLOB:
                        //value = Sqlite.sqlite3_column_type(ptr, rowIndex);
                        break;
                    case Sqlite.SQLITE_NULL:
                        value = null;
                        break;
                }

                recode[columnName] = value;
            }

            return recode;
        }
    }
}
