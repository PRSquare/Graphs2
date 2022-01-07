using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Utils
{
    public class FileUtils
    {
        public static string ReadFile(String path)
        {
            // Англ. формат записи чисел с  плавующей точкой (через точку, а не через запятую)
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            if (path == "")
                return null;
            StreamReader sr = new StreamReader(path);
            // Запись содержимого файла в буффер
            string inLine = sr.ReadLine();
            string buff = null;
            while (inLine != null)
            {
                buff += inLine + "\n";
                inLine = sr.ReadLine();
            }
            sr.Close();

            return buff;
        }
        
        public static void SaveToFile(string path, string buff)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            if (path == "")
                return;
            StreamWriter sr = new StreamWriter(path);

            sr.Write(buff);
            // Запись содержимого файла в буффер
            sr.Close();
        }
    }
}
