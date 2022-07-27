using System;
using System.Collections.Generic;
using System.Text;

namespace APeiLee.Tool
{
    public static class Md5Helper
    {
        public static string GetMd5(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(str);
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hash = md5.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }
    }
}
