using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictionResponseMessages
{
    public interface AnalyzeFusionIndexOfSingleImageResponse
    {
        string Path { get; }
        double FusionIndex { get; }
    }
}
