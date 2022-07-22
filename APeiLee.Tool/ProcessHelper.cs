using System;
using System.Diagnostics;
using System.Linq;

namespace APeiLee.Tool
{
    public static class ProcessHelper
    {
        public static bool FindProcess(string processName)
        {
            if (string.IsNullOrEmpty(processName))
            {
                throw new AggregateException("processName is null or empty.");
            }

            try
            {
                var processList = Process.GetProcessesByName(processName);

                return (processList?.Count() ?? 0) > 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public static void StartProcess(string appStartFilePath, string args)
        {
            try
            {
                if (!System.IO.File.Exists(appStartFilePath))
                {
                    throw new Exception($"启动文件不存在，信息:{appStartFilePath}");
                }

                var appInfo = new System.IO.FileInfo(appStartFilePath);
                var workingDir = appInfo.Directory?.FullName ?? "";

                Process appProcess = new Process();
                appProcess.StartInfo.UseShellExecute = false;
                appProcess.StartInfo.FileName = appStartFilePath;
                appProcess.StartInfo.Arguments = args;
                if (!string.IsNullOrEmpty(workingDir))
                {
                    appProcess.StartInfo.WorkingDirectory = workingDir;
                }
                appProcess.Start();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}
