using System;
using System.ComponentModel;
using Microsoft.ProjectOxford.Face;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
            faceServiceClient = new FaceServiceClient("63e227c7be5c47e0ae93c86f230e1c50", "https://southeastasia.api.cognitive.microsoft.com/face/v1.0");
        }

        public async void PhotoBtnClickAsync(object sender, EventArgs e)
        {
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
            });

            if(file != null)
            {
                libraryBitmap = SKBitmap.Decode(file.GetStream());

                await Navigation.PushAsync(new FaceInformationPage(libraryBitmap, faceServiceClient));
            }
        }

        public async void CameraBtnClickAsync(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() {
                PhotoSize = PhotoSize.Medium
            });
            
            if (photo != null)
            {
                libraryBitmap = SKBitmap.Decode(photo.GetStream());

                await Navigation.PushAsync(new FaceInformationPage(libraryBitmap, faceServiceClient));
            }
        }

        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            string text = "多媒體系統期末專題";

            var fontManager = SKFontManager.Default;
            var emojiTypeface = fontManager.MatchCharacter('時');

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                FakeBoldText = true,
                Color = SKColors.Blue,
                Typeface = emojiTypeface
            };

            // Adjust TextSize property so text is 95% of screen width
            float textWidth = textPaint.MeasureText(text);
            textPaint.TextSize = 0.95f * info.Width * textPaint.TextSize / textWidth;

            // Find the text bounds
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(text, ref textBounds);

            // Calculate offsets to center the text on the screen
            float xText = info.Width / 2 - textBounds.MidX;
            float yText = info.Height / 2 - textBounds.MidY;

            // And draw the text
            canvas.DrawText(text, xText, yText, textPaint);

            // Create a new SKRect object for the frame around the text
            SKRect frameRect = textBounds;
            frameRect.Offset(xText, yText);
            frameRect.Inflate(10, 10);

            // Create an SKPaint object to display the frame
            SKPaint framePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
                Color = SKColors.Blue
            };

            // Draw one frame
            canvas.DrawRoundRect(frameRect, 20, 20, framePaint);

            // Inflate the frameRect and draw another
            frameRect.Inflate(10, 10);
            framePaint.Color = SKColors.DarkBlue;
            canvas.DrawRoundRect(frameRect, 30, 30, framePaint);
        }

        public void OnCanvasViewPaintSurface1(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            string text = "Azure認知服務-臉部辨識";

            var fontManager = SKFontManager.Default;
            var emojiTypeface = fontManager.MatchCharacter('時');

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                FakeBoldText = true,
                Color = SKColors.Blue,
                Typeface = emojiTypeface
            };

            // Adjust TextSize property so text is 95% of screen width
            float textWidth = textPaint.MeasureText(text);
            textPaint.TextSize = 0.95f * info.Width * textPaint.TextSize / textWidth;

            // Find the text bounds
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(text, ref textBounds);

            // Calculate offsets to center the text on the screen
            float xText = info.Width / 2 - textBounds.MidX;
            float yText = info.Height / 2 - textBounds.MidY;

            // And draw the text
            canvas.DrawText(text, xText, yText, textPaint);

            // Create a new SKRect object for the frame around the text
            SKRect frameRect = textBounds;
            frameRect.Offset(xText, yText);
            frameRect.Inflate(10, 10);

            // Create an SKPaint object to display the frame
            SKPaint framePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
                Color = SKColors.Blue
            };

            // Draw one frame
            canvas.DrawRoundRect(frameRect, 15, 15, framePaint);

            // Inflate the frameRect and draw another
            frameRect.Inflate(10, 10);
            framePaint.Color = SKColors.DarkBlue;
            canvas.DrawRoundRect(frameRect, 25, 25, framePaint);
        }
    }
}
