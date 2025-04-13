using System;
using System.Collections.Generic;
using System.IO;

using filet_belated_8t.Tools.CreateBoxUrlShortcut.Extern;
using filet_belated_8t.Tools.CreateBoxUrlShortcut.Utility;
using System.Runtime.InteropServices;
using filet_belated_8t.Tools.CreateBoxUrlShortcut.Object;
using System.Text;
using System.Linq;

namespace filet_belated_8t.Tools.CreateBoxUrlShortcut
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // sync.db をコピー
            CopySyncDB();

            args = new string[] {
                @"C:\Users\k-min\Box\新しいフォルダー\test.xlsx"
                , @"C:\Users\k-min\Box\新しいフォルダー\test - コピー.xlsx"
            };
            // オンラインURLショートカットの作成
            CreateOnlineUrlShortcut(args);

            // オフラインURLショートカットの作成
            CreateOfflineUrlShortcut(args);

            // コピーしたsync.db を削除
            //RemoveSyncDBTemp();
        }

        private static void CopySyncDB()
        {
            // 内部変数を作成
            string dataDirectoryPath = Config.DataDirectoryPath;
            string tempDirectoryPath = Config.TempDirectoryPath;
            string syncDBFileName = Config.SyncDBFileName;

            // ディレクトリの作成
            if (Directory.Exists(tempDirectoryPath) == false)
            {
                Directory.CreateDirectory(tempDirectoryPath);
            }

            // コピー先ファイルの削除
            RemoveSyncDBTemp();

            // ファイルコピー
            foreach (string file in Directory.GetFiles(dataDirectoryPath, string.Format("{0}*", syncDBFileName)))
            {
                File.Copy(file, Path.Combine(tempDirectoryPath, Path.GetFileName(file)));
            }
        }

        private static void RemoveSyncDBTemp()
        {
            // 内部変数を作成
            string tempDirectoryPath = Config.TempDirectoryPath;
            string syncDBFileName = Config.SyncDBFileName;

            // 一時ディレクトリのファイルを削除
            foreach (string file in Directory.GetFiles(tempDirectoryPath, string.Format("{0}*", syncDBFileName)))
            {
                File.Delete(file);
            }
        }

        private static void CreateOnlineUrlShortcut(string[] filePaths)
        {
            // 内部変数を作成
            string currentDirectory = Environment.CurrentDirectory;
            string tempSyncDBPath = Path.Combine(Config.TempDirectoryPath, Config.SyncDBFileName);
            string boxOnlineShortcutBaseUrl = Config.BoxOnlineShortcutBaseUrl;
            string officeOnlineShortcutBaseUrl = Config.OfficeOnlineShortcutBaseUrl;
            string[] officeOnlineExtentions = Config.OfficeOnlineExtentions;

            // 
            Func<string, string> createQuery =
                (filePath) =>
                {
                    // 内部変数を作成
                    string syncDirectoryPath = Config.SyncDirectoryPath;

                    // syncディレクトリ配下確認
                    if (filePath.StartsWith(syncDirectoryPath) == false) throw new Exception();

                    // BoxItemのname単位に分割
                    string[] fileNames = filePath.Substring(syncDirectoryPath.Length).Split('\\');

                    string query = "";
                    query += @"SELECT parent0.box_id, parent0.item_type, parent0.parent_item_id, parent0.name, parent0.sort_name, parent0.owner_id, parent0.checksum, parent0.size, parent0.lock_id, parent0.lock_owner_id, parent0.content_created_at, parent0.content_updated_at, parent0.version_id, parent0.lock_app_type ";
                    query += @"FROM box_item parent0 ";
                    for (int parentIndex = 1; parentIndex < fileNames.Length; parentIndex++)
                    {
                        query += string.Format(@"INNER JOIN box_item parent{0} ON parent{0}.box_id = parent{1}.parent_Item_id ", parentIndex, parentIndex - 1);
                    }
                    query += @"WHERE ";
                    for (int parentIndex = 0; parentIndex < fileNames.Length; parentIndex++)
                    {
                        if (parentIndex != 0) query += @"AND ";
                        query += string.Format(@"parent{0}.name = '{1}' ", parentIndex, fileNames[fileNames.Length - parentIndex - 1]);
                    }
                    query += @"; ";

                    return query;
                };

            Dictionary<string, BoxItem[]> fileBoxItems = ExecQuery(filePaths, createQuery).ToDictionary(x => x.Key, x => BoxItem.FromRowDictionaly(x.Value));

            foreach (string filePath in fileBoxItems.Keys)
            {
                string fileName = Path.GetFileName(filePath);
                foreach (BoxItem boxItem in fileBoxItems[filePath])
                {
                    string urlFilePath = Path.Combine(currentDirectory, string.Format(@"{0} Web.url", fileName));
                    long index = 0;
                    while (File.Exists(urlFilePath) == true)
                    {
                        index++;
                        urlFilePath = Path.Combine(currentDirectory, string.Format(@"{0} Web ({1}).url", fileName, index));
                    }
                    Console.WriteLine(urlFilePath);

                    string extention = Path.GetExtension(boxItem.Name);
                    Console.WriteLine(
                        string.Join(
                            "\r\n"
                            , new string[] {
                                @"[InternetShortcut]"
                                , string.Format(string.Format(@"URL={0}", Array.Exists(officeOnlineExtentions, x => x == extention) ? officeOnlineShortcutBaseUrl: boxOnlineShortcutBaseUrl), boxItem.BoxId) 
                            }
                        )
                    );
                    Console.WriteLine();
                }
            }
        }

        private static void CreateOfflineUrlShortcut(string[] filePaths)
        {
            // 内部変数を作成
            string currentDirectory = Environment.CurrentDirectory;
            string syncDirectoryPath = Config.SyncDirectoryPath;
            string tempSyncDBPath = Path.Combine(Config.TempDirectoryPath, Config.SyncDBFileName);
            string officeOnlineShortcutBaseUrl = Config.OfficeOnlineShortcutBaseUrl;

            foreach (string filePath in filePaths)
            {
                // syncディレクトリ配下確認
                if (filePath.StartsWith(syncDirectoryPath) == false) throw new Exception();

                string fileName = Path.GetFileName(filePath);
                string urlFilePath = Path.Combine(currentDirectory, string.Format(@"{0}.url", fileName));
                long index = 0;
                while (File.Exists(urlFilePath) == true)
                {
                    index++;
                    urlFilePath = Path.Combine(currentDirectory, string.Format(@"{0} ({1}).url", fileName, index));
                }
                Console.WriteLine(urlFilePath);

                string extention = Path.GetExtension(filePath);
                string exeFile = GetAssociatedExecutable(extention);

                Console.WriteLine(
                    string.Join(
                        "\r\n"
                        , new string[]{ 
                            @"[InternetShortcut]"
                            , string.Format(@"URL=file:///{0}", filePath.Replace(@"\", @"/"))
                            , string.Format(@"IconFile={0}", exeFile)
                            , @"IconIndex=0" 
                        }
                    )
                );
                Console.WriteLine();
            }
        }

        private static Dictionary<string, Dictionary<string, object>[]> ExecQuery(string[] filePaths, Func<string, string> createQuery)
        {
            // 内部変数を作成
            string tempSyncDBPath = Path.Combine(Config.TempDirectoryPath, Config.SyncDBFileName);
            string syncDirectoryPath = Config.SyncDirectoryPath;

            // DBオープン
            IntPtr db;
            if (Sqlite.sqlite3_open(tempSyncDBPath, out db) != Sqlite.SQLITE_OK) throw new Exception();

            Dictionary<string, Dictionary<string, object>[]> fileRecodes = new Dictionary<string, Dictionary<string, object>[]>();
            try
            {
                foreach (string filePath in filePaths)
                {
                    // syncディレクトリ配下確認
                    if (filePath.StartsWith(syncDirectoryPath) == false) continue;

                    // BoxItemのname単位に分割
                    string[] fileNames = filePath.Substring(syncDirectoryPath.Length).Split('\\');

                    // SQLクエリを作成
                    string query;
                    try
                    {
                        query = createQuery(filePath);
                    }
                    catch
                    {
                        continue;
                    }

                    IntPtr queryPrt = query.ToIntPtr();
                    if (queryPrt == IntPtr.Zero) continue;
                    try
                    {
                        IntPtr stmHandle;
                        Sqlite.sqlite3_prepare_v2(db, queryPrt, queryPrt.GetSize(), out stmHandle, IntPtr.Zero);
                        try
                        {
                            List<Dictionary<string, object>> recodes = fileRecodes.ContainsKey(filePath) ? fileRecodes[filePath].ToList() : new List<Dictionary<string, object>>();
                            while (Sqlite.sqlite3_step(stmHandle) == Sqlite.SQLITE_ROW)
                            {
                                recodes.Add(stmHandle.RowToDictionaly());
                            }
                            fileRecodes[filePath] = recodes.ToArray();
                        }
                        finally
                        {
                            Sqlite.sqlite3_finalize(stmHandle);
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(queryPrt);
                    }
                }
            }
            finally
            {
                // DBクロース
                Sqlite.sqlite3_close(db);
            }

            return fileRecodes;
        }

        private static string GetAssociatedExecutable(string extension)
        {
            uint length = 0;
            Shlwapi.AssocQueryString(Shlwapi.ASSOCF_NONE, Shlwapi.ASSOCSTR_EXECUTABLE, extension, null, null, ref length);

            StringBuilder sb = new StringBuilder((int)length);
            uint ret = Shlwapi.AssocQueryString(Shlwapi.ASSOCF_NONE, Shlwapi.ASSOCSTR_EXECUTABLE, extension, null, sb, ref length);

            return ret == 0 ? sb.ToString() : null;
        }
    }
}
