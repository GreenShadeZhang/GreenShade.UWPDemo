using GreenShade.ML.EmoticonDetection.Services;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace GreenShade.ML.EmoticonDetection
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool _isInitialized = false;
        public MainPage()
        {
            CurrentEmojis._emojis = new EmojiCollection();
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
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
                await MessageDialogService.Current.WriteMessage(result.ToString() + GameText.LOADER.GetString("CameraHelperResultFailed"));
                return;
            }

            bool isModelLoaded = await IntelligenceService.Current.InitializeAsync();
            if (!isModelLoaded)
            {
                await MessageDialogService.Current.WriteMessage(GameText.LOADER.GetString("ModelLoadedFailed"));
                return;
            }
            IntelligenceService.Current.IntelligenceServiceEmotionClassified += Current_IntelligenceServiceEmotionClassified;
          
            _isInitialized = true;
        }

        public void Current_IntelligenceServiceEmotionClassified(object sender, ClassifiedEmojiEventArgs e)
        {
            //在这里就可以做自己的操作了
            CurrentEmojis._currentEmoji = e.ClassifiedEmoji;
            UpdateCarouselAsync();
        }

        public async void UpdateCarouselAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Face.Text = CurrentEmojis._currentEmoji.Name;
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
