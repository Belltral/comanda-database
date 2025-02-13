using System;
using System.IO;

namespace SharedComponents.Utils
{
    public static class Logger
    {
        public static void Log(string entry)
        {
            string defaultPath = @"D:";

            DateTime dateTime = DateTime.Now;

            using (FileStream fs = new FileStream($@"{defaultPath}\ServerLog.txt", File.Exists(defaultPath + "\\ServerLog.txt") ? FileMode.Append : FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"{dateTime}  {entry}");
                    sw.WriteLine("----------------------------------------");
                }
            }
        }
    }
}
