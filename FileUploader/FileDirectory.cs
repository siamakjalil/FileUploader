using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileUploader
{
    public static class FileDirectory
    {
        public static bool CheckExistsAndCreate(string path)
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
