using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictionRequestMessages
{
    public interface AnalyzeAngleOfSingleImageRequest
    {
        string PathToImage { get; }
        string StoragePath { get; }
    }
}
