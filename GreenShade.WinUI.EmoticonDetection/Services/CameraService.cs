// Copyright (c) Microsoft Corporation. 
// Licensed under the MIT license. 
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.MediaProperties;

namespace GreenShade.WinUI.EmoticonDetection.Services
{
    public class CameraService
    {
        public event EventHandler<SoftwareBitmapEventArgs> SoftwareBitmapFrameCaptured;

        private static CameraService _current;
        public static CameraService Current => _current ?? (_current = new CameraService());

        private bool _isInitialized = false;
        private bool _isProcessing = false;

        private MediaCapture mediaCapture = null;

        private void Current_IntelligenceServiceProcessingCompleted(object sender, EventArgs e)
        {
            _isProcessing = false;
        }

        public async Task InitializeAsync()
        {
            mediaCapture = new MediaCapture();

            mediaCapture.Failed += MediaCapture_Failed;

            await mediaCapture.InitializeAsync();
        }

        public async Task GetImageResultAsync()
        {
            // Prepare and capture photo
            var lowLagCapture = await mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));

            var capturedPhoto = await lowLagCapture.CaptureAsync();

            var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;

            SoftwareBitmapFrameCaptured?.Invoke(this, new SoftwareBitmapEventArgs(softwareBitmap));

            await lowLagCapture.FinishAsync();

            _isInitialized = true;
        }

        private void MediaCapture_Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            throw new NotImplementedException();
        }
    }
}
