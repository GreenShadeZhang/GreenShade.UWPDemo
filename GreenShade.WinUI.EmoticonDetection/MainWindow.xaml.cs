using GreenShade.WinUI.EmoticonDetection.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GreenShade.WinUI.EmoticonDetection
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            CurrentEmojis._emojis = new EmojiCollection();
            this.InitializeComponent();
        }

        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";

            await CameraService.Current.InitializeAsync();

            bool isModelLoaded = await IntelligenceService.Current.InitializeAsync();

            IntelligenceService.Current.IntelligenceServiceEmotionClassified += Current_IntelligenceServiceEmotionClassified;
        
            await CameraService.Current.GetImageResultAsync();
        }

        private void Current_IntelligenceServiceEmotionClassified(object sender, ClassifiedEmojiEventArgs e)
        {
            Result.Text = e.ClassifiedEmoji.Name;
        }
    }
}
