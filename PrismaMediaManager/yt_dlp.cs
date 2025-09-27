using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Prisma_Media_Manager
{
    class yt_dlp
    {
        //Static Variables
        const string DOWNLOAD_URL = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";
        static string exePath = Directory.GetCurrentDirectory() + @"\deps\yt-dlp.exe";
        //Object variables
        private string sourceUrl, outputPath, format;
        private string additionalParams;

        public static void CheckDependencies()
        {
            if (!File.Exists(exePath))
            {
                var download = MessageBox.Show("yt-dlp.exe could not be found. Would you like to download it now?",
                    "Prisma Media Manager", MessageBoxButton.YesNo, MessageBoxImage.Error);

                if (download == MessageBoxResult.Yes)
                    Process.Start(new ProcessStartInfo(DOWNLOAD_URL) { UseShellExecute = true });
                MessageBox.Show("Please download yt-dl.exe and put it in " + Directory.GetCurrentDirectory() + @"\deps",
                        "Prisma Media Manager", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public yt_dlp(string url, string path, string type)
        {
            sourceUrl = url;
            outputPath = path;
            format = type;
        }

        public void SetAdditionalParameters(string p)
        {
            additionalParams = p;
        }

        public void DownloadVideo(System.Windows.Controls.TextBlock output)
        {
            //Construct the command string
            string commandString = $"{sourceUrl} --recode-video {format} -o \"{outputPath}\"";
            if (additionalParams != null)
                commandString += " " + additionalParams;

            string readError;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = exePath;
            startInfo.Arguments = commandString;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = false;
            process.StartInfo = startInfo;
            process.Start();
            StreamReader reader = process.StandardError;
            process.WaitForExit();
            if(File.Exists(outputPath))
            {
                string fileName = Path.GetFileNameWithoutExtension(outputPath);
                new ToastContentBuilder()
                    //.AddArgument("fileFolder", Path.GetDirectoryName(outputPath))
                    .AddText("Download complete!")
                    .AddText($"{fileName} has finished downloading successfully!")
                    .Show();

                ToastNotificationManagerCompat.OnActivated += toastArgs =>
                {
                    // Obtain the arguments from the notification
                    //ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                    // Need to dispatch to UI thread if performing UI operations
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        // TODO: Show the corresponding content
                        Process.Start("explorer.exe", $"/select,\"{outputPath}\"");
                    });
                };
            }
            else
            {
                readError = reader.ReadLine();
                if (!(readError == null))
                {
                    MessageBox.Show(readError, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
