using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Prisma_Media_Manager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        CheckDependencies();
    }

    private void CheckDependencies()
    {
        FFmpeg.CheckDependencies();
        yt_dlp.CheckDependencies();
    }

    private void ManageFormatBox(ComboBox c, string mode)
    {
        List<string> VIDEO_TYPES = new List<string> { "mp4", "avi", "mov", "flp", "webm" };
        List<string> AUDIO_TYPES = new List<string> { "mp3", "wav", "ogg", "opus", "aac", "flac" };

        if(mode == "Video")
            c.ItemsSource = VIDEO_TYPES;
        else if(mode == "Audio")
            c.ItemsSource = AUDIO_TYPES;

        c.SelectedIndex = 0;
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        var radioButton = sender as RadioButton;
        if (radioButton != null && radioButton.IsChecked == true)
        {
            string mode = radioButton.Content.ToString();
            if (radioButton.GroupName == "DownloadOutputType")
                ManageFormatBox(downloadFormat, mode);
            else if (radioButton.GroupName == "ConvertOutputType")
                ManageFormatBox(convertFormat, mode);
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (ValidateDownloadFields())
        {
            string url = urlBox.Text;
            string format = downloadFormat.Text;
            var saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.DefaultExt = $".{format}";
            saveDialog.Filter = $"Media file (.{format})|*.{format}";

            if (saveDialog.ShowDialog() == true)
            {
                yt_dlp download = new yt_dlp(urlBox.Text, saveDialog.FileName, format);
                download.DownloadVideo(debugBox);
            }
        }
    }

    private bool ValidateDownloadFields()
    {
        bool result = false;
        if (urlBox.Text == "URL" || urlBox.Text.Replace(" ", "") == "")
            MessageBox.Show("Please enter a URL!", "Prisma Media Manager", MessageBoxButton.OK, MessageBoxImage.Error);
        else if (downloadFormat.Text == "URL" || downloadFormat.Text.Replace(" ", "") == "")
            MessageBox.Show("Please select a file format!", "Prisma Media Manager", MessageBoxButton.OK, MessageBoxImage.Error);
        else
            result = true;
        return result;
    }
}