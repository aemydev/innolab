using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictionRequestMessages
{
    public interface AnalyzeFusionIndexOfSingleImageRequest
    {
        string PathToImage { get; }
        string StoragePath { get; }
    }
}
