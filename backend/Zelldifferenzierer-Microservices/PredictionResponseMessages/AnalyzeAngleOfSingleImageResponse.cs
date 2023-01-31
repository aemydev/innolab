using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictionResponseMessages
{
    public interface AnalyzeAngleOfSingleImageResponse
    {
        string Path { get; }
        float Angle { get; }
    }
}
