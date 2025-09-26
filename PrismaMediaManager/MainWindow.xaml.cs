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
using Microsoft.Win32;
using System.Diagnostics;

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
        List<string> VIDEO_TYPES = [".mp4", ".avi", "mov", "flp", "webm"];
        List<string> AUDIO_TYPES = [".mp3", ".wav", ".ogg", "opus", "aac", "flac"];

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
}