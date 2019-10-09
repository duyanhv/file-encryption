using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RivestCipher.Helper
{
    class CanReadFile
    {
        const int ERROR_SHARING_VIOLATION = 32;
        const int ERROR_LOCK_VIOLATION = 33;
        private static bool IsFileLocked(Exception exception)
        {
            int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
        }
        internal static bool Check(string filePath)
        {
            try
            {
                using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    if (fileStream != null) fileStream.Close();
                }
            }
            catch (IOException ex)
            {
                if (IsFileLocked(ex))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
