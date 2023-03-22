namespace RobotControl.ClassLibrary.ImageRecognition
{
    public interface IImageRecognitionFromCamera : IDisposable
    {
        bool Open(string cameraId);
        ImageRecognitionFromCameraResult Get(string[] labelsOfObjectsToDetect);
    }
}