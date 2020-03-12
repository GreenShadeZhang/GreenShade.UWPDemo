using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace GreenShade.ML.OnnxModel
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private mnistModel ModelGen;
        private mnistInput ModelInput = new mnistInput();
        private mnistOutput ModelOutput;
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await LoadModelAsync();
        }
        private async Task LoadModelAsync()
        {
            try
            {
                // Load a machine learning model
                StorageFile modelFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/mnist.onnx"));
                ModelGen = await mnistModel.CreateFromStreamAsync(modelFile as IRandomAccessStreamReference);
            }
            catch(Exception ex)

            {

            }
           
        }

        private async void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            // Trigger file picker to select an image file
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            StorageFile selectedStorageFile = await fileOpenPicker.PickSingleFileAsync();

            SoftwareBitmap softwareBitmap;
            using (IRandomAccessStream stream = await selectedStorageFile.OpenAsync(FileAccessMode.Read))
            {
                // Create the decoder from the stream 
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Get the SoftwareBitmap representation of the file in BGRA8 format
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }

            // Display the image
            //SoftwareBitmapSource imageSource = new SoftwareBitmapSource();
            //await imageSource.SetBitmapAsync(softwareBitmap);
            //UIPreviewImage.Source = imageSource;

            // Encapsulate the image within a VideoFrame to be bound and evaluated
            VideoFrame inputImage = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);
            ModelInput.input_placeholer00 = ImageFeatureValue.CreateFromVideoFrame(inputImage);
            //Evaluate the model
            ModelOutput = await ModelGen.EvaluateAsync(ModelInput);

            //Convert output to datatype
            IReadOnlyList<float> vectorImage = ModelOutput.stage_30mid_conv70BiasAdd00.GetAsVectorView();
            IList<float> imageList = vectorImage.ToList();

            //LINQ query to check for highest probability digit
            var maxIndex = imageList.IndexOf(imageList.Max());

            //Display the results
            Result.Text = maxIndex.ToString();
        }
    }
}
