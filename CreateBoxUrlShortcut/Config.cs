using System;
using System.IO;
using Microsoft.Win32;

namespace filet_belated_8t.Tools.CreateBoxUrlShortcut
{
    internal class Config
    {
        /// <summary>
        /// 同期ディレクトリパス
        /// </summary>
        public static string SyncDirectoryPath { get; private set; }

        /// <summary>
        /// box driveのデータディレクトリパス
        /// </summary>
        public static string DataDirectoryPath { get; private set; }

        /// <summary>
        /// 一時ディレクトリパス
        /// </summary>
        public static string TempDirectoryPath { get; private set; }

        /// <summary>
        /// boxの同期情報DBファイル名
        /// </summary>
        public static string SyncDBFileName { get; private set; }

        /// <summary>
        /// boxオンラインShortcutURL
        /// </summary>
        public static string BoxOnlineShortcutBaseUrl { get; private set; }

        /// <summary>
        /// OfficeオンラインShortcutURL
        /// </summary>
        public static string OfficeOnlineShortcutBaseUrl { get; private set; }

        /// <summary>
        /// Officeオンライン拡張子
        /// </summary>
        public static string[] OfficeOnlineExtentions { get; private set; }

        static Config()
        {
            Reload();
        }

        public static void Reload()
        {
            SyncDirectoryPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Box\Box\preferences", "sync_directory_path", null);
            if (SyncDirectoryPath.EndsWith(@"\") == false) SyncDirectoryPath = string.Format(@"{0}\", SyncDirectoryPath);

            DataDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Box\Box\data");
            TempDirectoryPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Temp");
            SyncDBFileName = @"sync.db";
            BoxOnlineShortcutBaseUrl = @"https//:box.com/{0}/";
            OfficeOnlineShortcutBaseUrl = @"https://office.com/{0}/";
            OfficeOnlineExtentions = new string[] { "docx", "xlsx", "pptx" };
            for (int i = 0; i < OfficeOnlineExtentions.Length; i++)
            {
                if (OfficeOnlineExtentions[i].StartsWith(".") == false) OfficeOnlineExtentions[i] = string.Format(@".{0}", OfficeOnlineExtentions[i]);
            }
        }
    }
}
