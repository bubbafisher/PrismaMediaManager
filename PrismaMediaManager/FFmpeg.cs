using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Prisma_Media_Manager
{
    class FFmpeg
    {
        //Static Variables
        static string exePath = Directory.GetCurrentDirectory() + @"\deps\ffmpeg.exe";
        //Object variables
        private string inputFile, outputFile, format;
        public static void CheckDependencies()
        {
            if (!File.Exists(exePath))
                MessageBox.Show("FFmpeg could not be found. Please reinstall the application.",
                    "Prisma Media Manager", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public FFmpeg(string input, string output, string type)
        {
            inputFile = input;
            outputFile = output;
            format = type;
        }

        public void ConvertFile()
        {
            //Construct the command string
            string commandString = $"-i \"{inputFile}\" \"{outputFile}\"";

            //TODO: Implement logging for FFmpeg
            string readError;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = exePath;
            startInfo.Arguments = commandString;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.CreateNoWindow = false;
            process.StartInfo = startInfo;
            process.Start();

            //TODO: Implement output redirection
            //StreamReader reader = process.StandardOutput;

            process.WaitForExit();

            if (File.Exists(outputFile))
            {
                string fileName = Path.GetFileNameWithoutExtension(outputFile);
                new ToastContentBuilder()
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
                        Process.Start("explorer.exe", $"/select,\"{outputFile}\"");
                    });
                };
            }
            //TODO: Implement error handling and recognition
            else
            {
                MessageBox.Show("Something went wrong with converting the file! Please make sure the input file is a valid media file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
