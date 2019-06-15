using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentifyFaces.Interface;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace IdentifyFaces
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        SKBitmap libraryBitmap;
        FaceServiceClient faceServiceClient;

        public MainPage()
        {
            InitializeComponent();
            faceServiceClient = new FaceServiceClient("2336c328ee8b42f8b5141d3c9aebd1ec", "https://southeastasia.api.cognitive.microsoft.com/face/v1.0");
        }

        public async void PhotoBtnClickAsync(object sender, EventArgs e)
        {
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,

            });

            if(file != null)
            {
                libraryBitmap = SKBitmap.Decode(file.GetStream());
                canvasView.InvalidateSurface();
            }

            await Navigation.PushAsync(new FaceInformationPage(libraryBitmap, faceServiceClient));
        }

        public async void CameraBtnClickAsync(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
            {
                PhotoSize = PhotoSize.Medium,
            });

            if (photo != null)
            {
                libraryBitmap = SKBitmap.Decode(photo.GetStream());
                canvasView.InvalidateSurface();


                //await Navigation.PushAsync(new FaceInformationPage(libraryBitmap, faceServiceClient));
            }
        }

        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (libraryBitmap != null)
            {
                var resizedBitmap = libraryBitmap.Resize(info, SKFilterQuality.High);
                canvas.DrawBitmap(resizedBitmap, info.Width / 2 - resizedBitmap.Width / 2, info.Height / 2 - resizedBitmap.Height / 2);
            }
        }
    }
}
