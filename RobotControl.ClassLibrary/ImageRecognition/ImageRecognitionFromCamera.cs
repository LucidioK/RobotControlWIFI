using Microsoft.ML;
using Microsoft.ML.Data;

using OpenCvSharp;
using OpenCvSharp.Extensions;
using RobotControl.ClassLibrary.ONNXImplementation;
using SkiaSharp;
using System.Drawing;

namespace RobotControl.ClassLibrary.ImageRecognition
{
    internal class ImageRecognitionFromCamera : IImageRecognitionFromCamera
    {
        //VideoCaptureAPIs.V4L2; // Cannot open rtsp stream 
        //VideoCaptureAPIs.GSTREAMER; // Cannot open rtsp stream  
        //VideoCaptureAPIs.ANY; // 2sec delay 
        //VideoCaptureAPIs.OPENCV_MJPEG; // Cannot open rtsp stream 
        //VideoCaptureAPIs.FFMPEG; // 2sec delay;
        private const VideoCaptureAPIs videoCaptureAPI = VideoCaptureAPIs.FFMPEG;
        private TinyYoloModel tinyYoloModel;
        private OnnxModelConfigurator onnxModelConfigurator;
        private OnnxOutputParser onnxOutputParser;
        private PredictionEngine<ImageInputData, TinyYoloPrediction> tinyYoloPredictionEngine;
        private VideoCapture videoCapture;
        private Thread retrieveFramesFromVideoCaptureThread;
        private Mat latestFrame = new Mat();
        private Mat emptyFrame = new Mat();
        public ImageRecognitionFromCamera(int GPUDeviceId)
        {
            var onnxFilePath = Directory.EnumerateFiles(".", "*.onnx").First();
            if (string.IsNullOrEmpty(onnxFilePath))
            {
                throw new FileNotFoundException($"Could not find any onnx file in the current folder {Directory.GetCurrentDirectory()}");
            }
            tinyYoloModel = new TinyYoloModel(onnxFilePath);
            onnxModelConfigurator = new OnnxModelConfigurator(tinyYoloModel, GPUDeviceId);
            onnxOutputParser = new OnnxOutputParser(tinyYoloModel);
            tinyYoloPredictionEngine = onnxModelConfigurator.GetMlNetPredictionEngine<TinyYoloPrediction>();
        }

        public bool Open(string rtspUrl)
        {
            var taskopened = OpenIPCameraAsync(rtspUrl);
            taskopened.Wait();
            var opened = taskopened.Result;

            if (!opened)
            {
                throw new ArgumentException($"Could not open camera {rtspUrl}");
            }

            return opened;
        }

        public ImageRecognitionFromCameraResult Get(IList<string> labelsOfObjectsToDetect)
        {
            ImageRecognitionFromCameraResult result = GetEmptyImageRecognitionFromCameraResult();

            Mat frame = latestFrame;

            if (frame == null || frame.Empty())
            {
                return result;
            }

            frame.Flip(FlipMode.Y);
            result.Bitmap = frame.ToBitmap();
            float[] labels = Predict(frame);
            var filteredBoxes = onnxOutputParser
                                .FilterBoundingBoxes(onnxOutputParser.ParseOutputs(labels), 5, 0.5f)
                                .Where(b => labelsOfObjectsToDetect.Any(l => l.Equals(b.Label, StringComparison.InvariantCultureIgnoreCase)))
                                .ToList();
            if (filteredBoxes.Count > 0)
            {
                PopulateResult(result, filteredBoxes);
            }

            return result;
        }

        public void Dispose()
        {
            videoCapture?.Dispose();
            tinyYoloPredictionEngine?.Dispose();
        }

        private static void PopulateResult(ImageRecognitionFromCameraResult result, List<BoundingBox> filteredBoxes)
        {
            var highestConfidence    = filteredBoxes.Select(b => b.Confidence).Max();
            var highestConfidenceBox = filteredBoxes.First(b => b.Confidence == highestConfidence);
            var bbdfb                = BoundingBoxDeltaFromBitmap.FromBitmap(result.Bitmap.Width, result.Bitmap.Height, highestConfidenceBox);
            var dimensions           = HighlightDetectedObject(result.Bitmap, highestConfidenceBox, bbdfb);
            result.HasData           = true;
            result.Label             = dimensions + $", label={highestConfidenceBox.Label}";
            result.XDeltaProportionFromBitmapCenter 
                                     = bbdfb.XDeltaProportionFromBitmapCenter;
        }

        private ImageRecognitionFromCameraResult GetEmptyImageRecognitionFromCameraResult() =>
            new ImageRecognitionFromCameraResult
            {
                HasData = false,
                ImageRecognitionFromCamera = this,
            };

        private void PrintTimeSpans(IList<DateTime> times, IList<string> labels)
        {
            string s = "";
            for (var i = 1; i < times.Count; i++)
            {
                var elapsed = times[i] - times[i - 1];
                s += labels[i] + ":" + (int)elapsed.TotalMicroseconds + " ";
            }
            System.Diagnostics.Debug.WriteLine(s);
        }

        private float[] Predict(Mat frame) =>
            tinyYoloPredictionEngine
                .Predict(new ImageInputData { Image = MLImage.CreateFromStream(frame.ToMemoryStream()) })
                .PredictedLabels;

        private async Task<bool> OpenIPCameraAsync(string cameraId)
        {
            bool opened;
            videoCapture = new VideoCapture(cameraId, videoCaptureAPI);
            opened = videoCapture.Open(cameraId, videoCaptureAPI);
            if (opened)
            {

                retrieveFramesFromVideoCaptureThread = new Thread(RetrieveFramesFromVideoCaptureThreadProc) { Priority = ThreadPriority.AboveNormal };
                retrieveFramesFromVideoCaptureThread.Start();
            }

            return opened;
        }

        private static Font highlightFont = new Font(FontFamily.GenericMonospace, 15.0f);
        private static Brush highlightTextBrush = new SolidBrush(Color.Yellow);
        private static PointF highlightTextPosition = new PointF(64.0f, 64.0f);
        private static Pen redPen = new Pen(Color.Red, 3);
        private static Pen greenPen = new Pen(Color.Green, 2);
        private static string HighlightDetectedObject(Bitmap bitmap, BoundingBox box, BoundingBoxDeltaFromBitmap bbdfb)
        {
            var x = box.Dimensions.X * bbdfb.CorrX;
            var y = box.Dimensions.Y * bbdfb.CorrY;
            var w = box.Dimensions.Width * bbdfb.CorrX;
            var h = box.Dimensions.Height * bbdfb.CorrY;
            highlightTextPosition.X = bitmap.Width - 256;
            var midX = x + w / 2;
            var midY = y + h / 2;
            if (w != 0)
            {
                using (var gr = Graphics.FromImage(bitmap))
                {
                    gr.DrawRectangle(redPen, x, y, w, h);
                    gr.DrawLine(greenPen, 0, midY, bitmap.Width - 1, midY);
                    gr.DrawLine(greenPen, midX, 0, midX, bitmap.Height - 1);
                    gr.DrawString(DateTime.Now.ToString(), highlightFont, highlightTextBrush, highlightTextPosition);
                }
            }

            return $"x:{(int)x}, y:{(int)y}, w:{(int)w}, h:{(int)h}";
        }

        private void RetrieveFramesFromVideoCaptureThreadProc(object obj)
        {
            while (true)
            {
                latestFrame = videoCapture.RetrieveMat();
            }
        }
    }
}
