using MassTransit;
using PredictionRequestMessages;
using PredictionResponseMessages;
using PredictionService.Logic;

namespace PredictionService.Consumers
{
    public class CalculateFusionIndexOFSingleImageConsumer : IConsumer<AnalyzeFusionIndexOfSingleImageRequest>
    {
        public async Task Consume(ConsumeContext<AnalyzeFusionIndexOfSingleImageRequest> context)
        {
            try
            {
                if (!File.Exists(context.Message.PathToImage)) throw new FileNotFoundException();
                var res = ClassicAnalyzer.FusionIndexCalculation(context.Message.PathToImage);

                Guid pictureId = Guid.NewGuid();

                string savePath = context.Message.StoragePath + "/angleResult_" + pictureId.ToString();
                res.Item1.Save(savePath);

                await context.RespondAsync<AnalyzeFusionIndexOfSingleImageResponse>(new { Path = "angleResult", FusionIndex = res.Item2 });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
