using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Prisma_Media_Manager
{
    class yt_dlp
    {
        static string exePath = Directory.GetCurrentDirectory() + @"\deps\yt-dlp.exe";
        public static void CheckDependencies()
        {
            if (!File.Exists(exePath))
            {
                var download = MessageBox.Show("yt-dlp.exe could not be found. Would you like to download it now?",
                    "Prisma Media Manager", MessageBoxButton.YesNo, MessageBoxImage.Error);

                if(download == MessageBoxResult.Yes)
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile("https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe",
                            exePath);
                    }
                }
                else
                {
                    MessageBox.Show("Please download yt-dl.exe and put it in " + Directory.GetCurrentDirectory() + @"\deps",
                        "Prisma Media Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
