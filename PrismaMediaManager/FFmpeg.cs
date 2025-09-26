using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Prisma_Media_Manager
{
    class FFmpeg
    {
        static string exePath = Directory.GetCurrentDirectory() + @"\deps\ffmpeg.exe";
        public static void CheckDependencies()
        {
            if(!File.Exists(exePath))
                MessageBox.Show("FFmpeg could not be found. Please reinstall the application.", 
                    "Prisma Media Manager", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
