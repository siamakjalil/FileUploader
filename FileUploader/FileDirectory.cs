using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileUploader
{
    public static class FileDirectory
    {
        public static bool Exist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return false;
            }
            return true;
        }
        public static bool Create(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
