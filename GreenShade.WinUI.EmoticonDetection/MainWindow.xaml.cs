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
using System.Threading.Tasks;
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
        private bool _isInitialized = false;
        public MainWindow()
        {
            CurrentEmojis._emojis = new EmojiCollection();
            this.InitializeComponent();
        }

        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";

            //await CameraService.Current.InitializeAsync();

            //bool isModelLoaded = await IntelligenceService.Current.InitializeAsync();

            //IntelligenceService.Current.IntelligenceServiceEmotionClassified += Current_IntelligenceServiceEmotionClassified;

            //await CameraService.Current.GetImageResultAsync();

            if (_isInitialized)
            {
                await CleanUpAsync();
            }
            await InitializeScreenAsync();
        }

        private async Task InitializeScreenAsync()
        {
            CameraHelperResult result = await CameraService.Current.InitializeAsync();
            if (result != CameraHelperResult.Success)
            {
               // await MessageDialogService.Current.WriteMessage(result.ToString() + GameText.LOADER.GetString("CameraHelperResultFailed"));
                return;
            }

            bool isModelLoaded = await IntelligenceService.Current.InitializeAsync();
            if (!isModelLoaded)
            {
               // await MessageDialogService.Current.WriteMessage(GameText.LOADER.GetString("ModelLoadedFailed"));
                return;
            }
            IntelligenceService.Current.IntelligenceServiceEmotionClassified += Current_IntelligenceServiceEmotionClassified;

            _isInitialized = true;
        }

        private void Current_IntelligenceServiceEmotionClassified(object sender, ClassifiedEmojiEventArgs e)
        {
            this.DispatcherQueue.TryEnqueue(() =>
            {
                Result.Text = e.ClassifiedEmoji.Name;
            });
            
        }

        private async Task CleanUpAsync()
        {
            _isInitialized = false;

            await CameraService.Current.CleanUpAsync();

            IntelligenceService.Current.CleanUp();

            IntelligenceService.Current.IntelligenceServiceEmotionClassified -= Current_IntelligenceServiceEmotionClassified;

            CurrentEmojis._currentEmoji = null;
        }
    }
}
