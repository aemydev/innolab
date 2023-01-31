using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LogServiceModels;
using LogServiceRequestMessages;
using LogServiceResponseMessages;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PredictionRequestMessages;
using PredictionResponseMessages;
using Serilog;

namespace GatewayService.Controllers
{
    [Route("cellanalyzer/analyze")]
    //[Authorize]
    public class AnalyzeController : Controller
    {
        private readonly IRequestClient<AnalyzeAngleOfSingleImageRequest> _analyzeAngleSingleImageClient;
        private readonly IRequestClient<AnalyzeFusionIndexOfSingleImageRequest> _analyzeFusionIndexSingleImageClient;


        public AnalyzeController(IRequestClient<AnalyzeAngleOfSingleImageRequest> analyzeAngleSingleImageClient, IRequestClient<AnalyzeFusionIndexOfSingleImageRequest> analyzeFusionIndexSingleImageClient)
        {
            _analyzeFusionIndexSingleImageClient = analyzeFusionIndexSingleImageClient;
            _analyzeAngleSingleImageClient = analyzeAngleSingleImageClient;
        }


        [HttpGet("fusionindex")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(List<LogEntry>))]
        public async Task<IActionResult> CalculateFusionIndexClassic([FromQuery] string path, [FromQuery] string filePath)
        {
            var res = await _analyzeFusionIndexSingleImageClient.GetResponse<AnalyzeFusionIndexOfSingleImageResponse>(new { StoragePath = path, PathToImage = filePath });
            if (res == null) return StatusCode(500);
            return Ok(res);
        }


        [HttpGet("angle")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(List<LogEntry>))]
        public async Task<IActionResult> CalculateAngleClassic([FromQuery] string path, [FromQuery] string filePath)
        {
            var res = await _analyzeAngleSingleImageClient.GetResponse<AnalyzeAngleOfSingleImageResponse>(new { StoragePath = path, PathToImage = filePath });
            if (res == null) return StatusCode(500);
            return Ok(res);
        }
    }
}
