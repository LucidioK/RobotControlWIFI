using RobotControl.ClassLibrary.ImageRecognition;
using RobotControl.ClassLibrary.RobotCommunication;

namespace RobotControl.ClassLibrary
{
    public static class ClassFactory
    {
        public static IRobotCommunication CreateRobotCommunication(string ipAddress) => new RobotCommunication(ipAddress);
        public static IImageRecognitionFromCamera CreateImageRecognitionFromCamera(bool useGPU) => new ImageRecognitionFromCamera(useGPU);
    }
}
