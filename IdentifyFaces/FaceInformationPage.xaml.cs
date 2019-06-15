using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace IdentifyFaces
{
    public partial class FaceInformationPage : ContentPage
    {
        SKBitmap libraryBitmap;
        FaceServiceClient faceServiceClient;
        ObservableCollection<FaceResult> faceResults;

        public FaceInformationPage(SKBitmap bitmap, FaceServiceClient ServiceClient)
        {
            InitializeComponent();

            faceResults = new ObservableCollection<FaceResult>();
            faceListView.ItemsSource = faceResults;
            libraryBitmap = bitmap;
            canvasView.InvalidateSurface();
            faceServiceClient = ServiceClient;

            IdentificationAsync();
        }

        private async void IdentificationAsync()
        {
            string personGroupId = "1421";

            var personGroup = await faceServiceClient.GetPersonGroupAsync(personGroupId);

            Face[] faces;
            Guid[] faceIds;

            SKImage image = SKImage.FromBitmap(libraryBitmap);
            SKData encoded = image.Encode();

            faces = await faceServiceClient.DetectAsync(encoded.AsStream(), true, true, new FaceAttributeType[] { FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Emotion});
            faceIds = faces.Select(face => face.FaceId).ToArray();

            if (faces.Count() > 0)
            {
                var results = await faceServiceClient.IdentifyAsync(personGroupId, faceIds);
                using (SKCanvas canvas = new SKCanvas(libraryBitmap))
                {
                    using (SKPaint paint = new SKPaint())
                    {
                        paint.Color = new SKColor(255, 0, 0);
                        paint.StrokeWidth = 5f;

                        foreach (var face in faces)
                        {
                            // 擷取臉部圖片
                            SKBitmap faceBitmap = new SKBitmap(face.FaceRectangle.Width, face.FaceRectangle.Height);
                            SKRect dest = new SKRect(0, 0, face.FaceRectangle.Width, face.FaceRectangle.Height);
                            SKRect source = new SKRect(face.FaceRectangle.Left, face.FaceRectangle.Top, face.FaceRectangle.Left + face.FaceRectangle.Width, face.FaceRectangle.Top + face.FaceRectangle.Height);

                            using (SKCanvas faceCanvas = new SKCanvas(faceBitmap))
                            {
                                faceCanvas.DrawBitmap(libraryBitmap, source, dest);
                            }

                            SKImage faceImage = SKImage.FromBitmap(faceBitmap);

                            List<(string, float)> emotions = new List<(string, float)>
                        {
                            ("生氣", face.FaceAttributes.Emotion.Anger),
                            ("鄙視", face.FaceAttributes.Emotion.Contempt),
                            ("厭惡", face.FaceAttributes.Emotion.Disgust),
                            ("恐懼", face.FaceAttributes.Emotion.Fear),
                            ("幸福", face.FaceAttributes.Emotion.Happiness),
                            ("中性", face.FaceAttributes.Emotion.Neutral),
                            ("悲傷", face.FaceAttributes.Emotion.Sadness),
                            ("驚喜", face.FaceAttributes.Emotion.Surprise)
                        };

                            var person = await faceServiceClient.GetPersonAsync(personGroupId, results.First(c => c.FaceId == face.FaceId).Candidates[0].PersonId);
                            string result = string.Format("名稱: {0}\n年紀: {1}\n眼鏡: {2}\n表情: {3}", person.Name, face.FaceAttributes.Age, face.FaceAttributes.Glasses, emotions.OrderByDescending(c => c.Item2).Select(c => c.Item1).First());

                            faceResults.Add(new FaceResult { Result = result, Face = ImageSource.FromStream(() => faceImage.Encode().AsStream()) });


                            // 把臉部框起來
                            canvas.DrawLine(new SKPoint(face.FaceRectangle.Left, face.FaceRectangle.Top), new SKPoint(face.FaceRectangle.Left + face.FaceRectangle.Width, face.FaceRectangle.Top), paint);
                            canvas.DrawLine(new SKPoint(face.FaceRectangle.Left, face.FaceRectangle.Top), new SKPoint(face.FaceRectangle.Left, face.FaceRectangle.Top + face.FaceRectangle.Height), paint);
                            canvas.DrawLine(new SKPoint(face.FaceRectangle.Left + face.FaceRectangle.Width, face.FaceRectangle.Top), new SKPoint(face.FaceRectangle.Left + face.FaceRectangle.Width, face.FaceRectangle.Top + face.FaceRectangle.Height), paint);
                            canvas.DrawLine(new SKPoint(face.FaceRectangle.Left, face.FaceRectangle.Top + face.FaceRectangle.Height), new SKPoint(face.FaceRectangle.Left + face.FaceRectangle.Width, face.FaceRectangle.Top + face.FaceRectangle.Height), paint);
                        }
                    }
                }

                canvasView.InvalidateSurface();
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

    public class FaceResult
    {
        public ImageSource Face { get; set; }
        public string Result { get; set; }
    }
}
