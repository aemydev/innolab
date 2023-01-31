using Emgu.CV.Stitching;
using MassTransit;
using PredictionRequestMessages;
using PredictionResponseMessages;
using PredictionService.Logic;

namespace PredictionService.Consumers
{
    public class CalculateAverageAngleOfSingleImageConsumer : IConsumer<AnalyzeAngleOfSingleImageRequest>
    {
        private ILogger<CalculateAverageAngleOfSingleImageConsumer> _logger;

        public async Task Consume(ConsumeContext<AnalyzeAngleOfSingleImageRequest> context)
        {
            try
            {
                if (!File.Exists(context.Message.PathToImage)) throw new FileNotFoundException();
                var res = ClassicAnalyzer.MeanAngleCalculation(context.Message.PathToImage);

                Guid pictureId = Guid.NewGuid();

                string savePath = context.Message.StoragePath + "\\angleResult_" + pictureId.ToString() + ".png";
                res.Item1.Save(savePath);

                await context.RespondAsync<AnalyzeAngleOfSingleImageResponse>(new{Path = savePath, Angle = res.Item2 });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
