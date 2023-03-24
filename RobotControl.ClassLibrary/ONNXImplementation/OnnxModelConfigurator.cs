using Microsoft.ML;
using Microsoft.ML.Transforms.Image;
using Microsoft.ML.Transforms.Onnx;

namespace RobotControl.ClassLibrary.ONNXImplementation
{
    internal class OnnxModelConfigurator
    {
        private readonly MLContext    mlContext = new MLContext();
        private readonly ITransformer mlModel;
        private readonly IDataView    dataView;

        public OnnxModelConfigurator(IOnnxModel onnxModel, int GPUDeviceId)
        {
            var onnxOptions = new OnnxOptions
            {
                FallbackToCpu     = true,
                GpuDeviceId       = GPUDeviceId,
                ModelFile         = onnxModel.ModelPath, 
                InputColumns      = new string[] { onnxModel.ModelInput},
                OutputColumns     = new string[] { onnxModel.ModelOutput},
                InterOpNumThreads = Environment.ProcessorCount / 2,
                IntraOpNumThreads = Environment.ProcessorCount / 2,
            };

            dataView = mlContext.Data.LoadFromEnumerable(new List<ImageInputData>());
            mlModel  = SetupMlNetModel(onnxModel, onnxOptions);
        }

        private ITransformer SetupMlNetModel(IOnnxModel onnxModel, OnnxOptions onnxOptions) =>
            mlContext
                .Transforms
                    .ResizeImages(
                        resizing:          ImageResizingEstimator.ResizingKind.Fill,
                        outputColumnName:  onnxModel.ModelInput,
                        imageWidth:        ImageSettings.imageWidth,
                        imageHeight:       ImageSettings.imageHeight,
                        inputColumnName:   nameof(ImageInputData.Image))

                    .Append(mlContext.Transforms.ExtractPixels(
                        outputColumnName:  onnxModel.ModelInput))

                    .Append(mlContext.Transforms.ApplyOnnxModel(
                        options: onnxOptions))

                    .Fit(
                        input: dataView);

        public PredictionEngine<ImageInputData, T> GetMlNetPredictionEngine<T>()
            where T : class, IOnnxObjectPrediction, new() => mlContext.Model.CreatePredictionEngine<ImageInputData, T>(mlModel);

        public void SaveMLNetModel(string mlnetModelFilePath) => mlContext.Model.Save(mlModel, null, mlnetModelFilePath);
    }
}
