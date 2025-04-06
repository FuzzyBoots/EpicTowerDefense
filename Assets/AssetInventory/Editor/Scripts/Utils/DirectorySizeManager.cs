using System.IO;
using System;
using System.Threading.Tasks;

namespace AssetInventory
{
    public class DirectorySizeManager
    {
        public bool Enabled = true;
        public bool IsRunning;
        public long CurrentSize;
        public DateTime LastCheckTime;

        private string _path;
        private long _byteLimit;
        private bool _isMonitoring;
        private readonly Func<string, bool> _validator;

        public DirectorySizeManager(string path, int gbLimit, Func<string, bool> validator)
        {
            _path = path;
            _validator = validator;

            SetLimit(gbLimit);
        }

        public void SetLimit(int gbLimit)
        {
            _byteLimit = gbLimit * 1024L * 1024L * 1024L;
        }

        public long GetLimit()
        {
            return _byteLimit;
        }

        public async void StartMonitoring(int scanPeriod)
        {
            _isMonitoring = true;
            while (_isMonitoring)
            {
                await Task.Delay(scanPeriod);
                if (!_isMonitoring) break;

                CheckAndClean();
            }
        }

        public void StopMonitoring()
        {
            _isMonitoring = false;
        }

        public async void CheckAndClean()
        {
            if (IsRunning || !Enabled) return;
            IsRunning = true;
            try
            {
                CurrentSize = await IOUtils.GetFolderSize(_path);
                if (CurrentSize > _byteLimit)
                {
                    string[] subDirs = Directory.GetDirectories(_path);
                    Array.Sort(subDirs, delegate(string a, string b)
                    {
                        DirectoryInfo aInfo = new DirectoryInfo(a);
                        DirectoryInfo bInfo = new DirectoryInfo(b);
                        return aInfo.CreationTime.CompareTo(bInfo.CreationTime);
                    });
                    int index = 0;
                    while (index < subDirs.Length && CurrentSize > _byteLimit)
                    {
                        long subDirSize = await IOUtils.GetFolderSize(subDirs[index]);

                        if (!Enabled) break;
                        if (!_validator(subDirs[index]))
                        {
                            index++;
                            continue;
                        }
                        await IOUtils.DeleteFileOrDirectory(subDirs[index]);

                        CurrentSize -= subDirSize;
                        index++;
                    }
                }
            }
            finally
            {
                IsRunning = false;
            }
            LastCheckTime = DateTime.Now;
        }
    }
}