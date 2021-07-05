using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Cookie_Editor.Services
{
    partial class Cookies_Editor : ServiceBase
    {
        private string _chrom_path_to_cookie;
        private FileSystemWatcher _chrome_watcher;

        private string _opera_path_to_cookie;
        private FileSystemWatcher _opera_watcher;

        private string _firefox_path_to_cookie;
        private FileSystemWatcher _firefox_watcher;


        private bool _enabled;
        public Cookies_Listener()
        {
            InitializeComponent();
            CanStop = true;
            CanShutdown = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        protected override void OnStart(string[] args) => Task.Run(() => Go());

        private void Go()
        {
            lock (this)
            {
                try
                {
                    _enabled = true;
                    try
                    {
                        _chrom_path_to_cookie = $@"C:\Users\{Get_User_Name()}\AppData\Local\Google\Chrome\User Data\Default";
                        _chrome_watcher = new FileSystemWatcher(_chrom_path_to_cookie); // тут мы укажем то за чем мы следим
                                                                                        // 

                        _chrome_watcher.Deleted += Watcher_Deleted;
                        _chrome_watcher.Created += Watcher_Created;
                        _chrome_watcher.Changed += Watcher_Changed;
                        _chrome_watcher.Renamed += Watcher_Renamed;

                        _chrome_watcher.EnableRaisingEvents = true;
                    }
                    catch (Exception ex) { EventLog.WriteEntry(ex.Message); }



                    try
                    {
                        _opera_path_to_cookie = $@"C:\Users\{Get_User_Name()}\AppData\Roaming\Opera Software\Opera Stable";
                        _opera_watcher = new FileSystemWatcher(_opera_path_to_cookie); // тут мы укажем то за чем мы следим
                                                                                       // 
                        _opera_watcher.Deleted += Watcher_Deleted;
                        _opera_watcher.Created += Watcher_Created;
                        _opera_watcher.Changed += Watcher_Changed;
                        _opera_watcher.Renamed += Watcher_Renamed;

                        _opera_watcher.EnableRaisingEvents = true;
                    }
                    catch (Exception ex) { EventLog.WriteEntry(ex.Message); }


                    try
                    {
                        _firefox_path_to_cookie = $@"{Get_FireFox_Cookies_Dir()}";
                        _firefox_watcher = new FileSystemWatcher(_firefox_path_to_cookie); // тут мы укажем то за чем мы следим
                                                                                           // 
                        _firefox_watcher.Deleted += Watcher_Deleted;
                        _firefox_watcher.Created += Watcher_Created;
                        _firefox_watcher.Changed += Watcher_Changed;
                        _firefox_watcher.Renamed += Watcher_Renamed;

                        _firefox_watcher.EnableRaisingEvents = true;
                    }
                    catch (Exception ex) { EventLog.WriteEntry(ex.Message); }

                    while (_enabled)
                    { Thread.Sleep(1000); }

                }
                catch (Exception ex) { EventLog.WriteEntry(ex.Message); }
            }
        }
        //получить директорию с куками фаер фокса так как они меняются в зависимости от версии
        private string Get_FireFox_Cookies_Dir()
        {
            string _FireFoxe_cookiesDB_name = "cookies.sqlite";
            string _path_to_FireFoxe = $@"C:\Users\{Get_User_Name()}\AppData\Roaming\Mozilla\Firefox\Profiles";

            try
            {
                var _path_array = Directory.GetFiles(_path_to_FireFoxe, _FireFoxe_cookiesDB_name, SearchOption.AllDirectories).ToList();
                if (_path_array.Count != 1)
                    throw new Exception("FireFoxe : cookies not fond =(");
                return _path_array[0].Replace("\\" + _FireFoxe_cookiesDB_name, string.Empty);
            }
            catch (Exception ex) { throw new Exception("FireFox : cookies not found"); }
        }

        // переименование файлов
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "переименован в " + e.FullPath;
            string filePath = e.OldFullPath;
            if (filePath.Contains(_chrom_path_to_cookie))
                Chrom_RecordEntry(fileEvent, filePath);
            if (filePath.Contains(_opera_path_to_cookie))
                Opera_RecordEntry(fileEvent, filePath);
            if (filePath.Contains(_firefox_path_to_cookie))
                FireFox_RecordEntry(fileEvent, filePath);
            else
                EventLog.WriteEntry("Он не понял куда записать");
        }
        // изменение файлов
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            if (filePath.Contains(_chrom_path_to_cookie))
                Chrom_RecordEntry(fileEvent, filePath);
            if (filePath.Contains(_opera_path_to_cookie))
                Opera_RecordEntry(fileEvent, filePath);
            if (filePath.Contains(_firefox_path_to_cookie))
                FireFox_RecordEntry(fileEvent, filePath);
            else
                EventLog.WriteEntry("Он не понял куда записать");
        }
        // создание файлов
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            if (filePath.Contains(_chrom_path_to_cookie))
                Chrom_RecordEntry(fileEvent, filePath);
            if (filePath.Contains(_opera_path_to_cookie))
                Opera_RecordEntry(fileEvent, filePath);
            if (filePath.Contains(_firefox_path_to_cookie))
                FireFox_RecordEntry(fileEvent, filePath);
            else
                EventLog.WriteEntry("Он не понял куда записать");
        }
        // удаление файлов
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "удален";
            string filePath = e.FullPath;
            if (filePath.Contains(  _chrom_path_to_cookie))
                Chrom_RecordEntry(fileEvent, filePath);
            if (filePath.Contains( _opera_path_to_cookie))
                Opera_RecordEntry(fileEvent, filePath);
            if (filePath.Contains(_firefox_path_to_cookie))
                FireFox_RecordEntry(fileEvent, filePath);
            else
                EventLog.WriteEntry("Он не понял куда записать");
        }

        public string Get_User_Name()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();
            return collection.Cast<ManagementBaseObject>().First()["UserName"].ToString().Split('\\')[1].Split('-').Last();
        }

        // запись в лог
        private void Chrom_RecordEntry(string fileEvent, string filePath)
        {
            string _name_log_file = @"\log_fo_Chrome.txt";
            using (StreamWriter writer = new StreamWriter($@"C:\Users\{Get_User_Name()}\Desktop" + _name_log_file, true))
            {
                EventLog.WriteEntry($@"C:\Users\{Get_User_Name()}\Desktop" + _name_log_file);
                writer.WriteLine(String.Format("{0} файл {1} был {2}",
                    DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                writer.Flush();
            }
        }
        private void FireFox_RecordEntry(string fileEvent, string filePath)
        {
            string _name_log_file = @"\log_fo_FireFox.txt";
            using (StreamWriter writer = new StreamWriter($@"C:\Users\{Get_User_Name()}\Desktop" + _name_log_file, true))
            {
                EventLog.WriteEntry($@"C:\Users\{Get_User_Name()}\Desktop" + _name_log_file);
                writer.WriteLine(String.Format("{0} файл {1} был {2}",
                    DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                writer.Flush();
            }
        }
        private void Opera_RecordEntry(string fileEvent, string filePath)
        {
            string _name_log_file = @"\log_fo_Opera.txt";
            using (StreamWriter writer = new StreamWriter($@"C:\Users\{Get_User_Name()}\Desktop" + _name_log_file, true))
            {
                EventLog.WriteEntry($@"C:\Users\{Get_User_Name()}\Desktop" + _name_log_file);
                writer.WriteLine(String.Format("{0} файл {1} был {2}",
                    DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                writer.Flush();
            }
        }
        protected override void OnStop()
        {
            _firefox_watcher.EnableRaisingEvents = false;
            _opera_watcher.EnableRaisingEvents = false;
            _chrome_watcher.EnableRaisingEvents = false;
            _enabled = false;
        }
    }
}
