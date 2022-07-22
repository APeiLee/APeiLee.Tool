using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace APeiLee.Tool
{
    public static class VersionReaderHelper
    {
        /// <summary>
        /// 获取指定路径Dll的版本信息
        /// </summary>
        /// <param name="dllFilePath"></param>
        /// <returns></returns>
        public static Version? GetDllVersion(string dllFilePath)
        {
            try
            {
                if (!File.Exists(dllFilePath))
                {
                    throw new Exception("获取Dll版本号时，Dll文件不存在");
                }

                AssemblyName assemblyName = AssemblyName.GetAssemblyName(dllFilePath);

                Version dllVersion = assemblyName.Version;

                return dllVersion;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);


                return null;
            }
        }

        /// <summary>
        /// 获取当前App版本信息
        /// </summary>
        /// <returns></returns>
        public static Version? GetAppVersion()
        {
            try
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }
    }
}
