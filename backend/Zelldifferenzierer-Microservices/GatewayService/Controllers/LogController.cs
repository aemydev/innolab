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
using Serilog;

namespace GatewayService.Controllers
{
    [Route("cellanalyzer/logs")]
    //[Authorize]
    public class LogController : Controller
    {
        private const string AdminId = "placeholder";
        private readonly IRequestClient<GetAllLogsRequest> _allLogsClient;
        private readonly IRequestClient<GetLogsByDateAfter> _logsByDateAfterClient;
        private readonly IRequestClient<GetLogsByDateBefore> _logsByDateBeforeClient;
        private readonly IRequestClient<GetLogsByDateBetween> _logsByDateBetweenClient;
        private readonly IRequestClient<GetLogsByLevelRequest> _getLogsByLevelClient;


        public LogController(IRequestClient<GetAllLogsRequest> allLogsClient, IRequestClient<GetLogsByDateAfter> logsByDateAfterClient,
            IRequestClient<GetLogsByDateBefore> logsByDateBeforeClient, IRequestClient<GetLogsByDateBetween> logsByDateBetweenClient,
            IRequestClient<GetLogsByLevelRequest> getLogsByLevelClient)
        {
            _allLogsClient = allLogsClient;
            _logsByDateAfterClient = logsByDateAfterClient;
            _logsByDateBeforeClient = logsByDateBeforeClient;
            _logsByDateBetweenClient = logsByDateBetweenClient;
            _getLogsByLevelClient = getLogsByLevelClient;
        }



        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(List<LogEntry>))]
        public async Task<IActionResult> GetAllLogs()
        {
            try
            {
                //For Authentification ! Currently placeholder
                //var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userId = AdminId;
                var t = _allLogsClient.Create(new { AdminId = userId });
                var res = await t.GetResponse<LogListResponse>();
                if (res == null) return StatusCode(500);
                return Ok(res);
            }
            catch (Exception e)
            {
                Log.Error($"Exception thrown in LogController -> GetAllLogs  Message : {e}");
                return StatusCode(500);
            }

        }

        [HttpGet("/logsAfter")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(List<LogEntry>))]
        public async Task<IActionResult> GetLogsByDateAfter([FromQuery(Name = "after")]  DateTime date)
        {
            //For Authentification !
            //var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = AdminId;
            var res = await _logsByDateAfterClient.GetResponse<LogListResponse>(new { AdminId = userId, Date = date });
            if (res == null) return StatusCode(500);
            return Ok(res);
        }

        [HttpGet("/logsBefore")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(List<LogEntry>))]
        public async Task<IActionResult> GetLogsByDateBefore([FromQuery(Name = "before")]  DateTime date)
        {
            //For Authentification !
            //var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = AdminId;
            var res = await _logsByDateBeforeClient.GetResponse<LogListResponse>(new { AdminId = userId, Date = date });
            if (res == null) return StatusCode(500);
            return Ok(res);
        }

        [HttpGet("/logsBetween")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(List<LogEntry>))]
        public async Task<IActionResult> GetLogsByDateBetween([FromQuery(Name = "after")]  DateTime lower, [FromQuery(Name = "before")]  DateTime upper)
        {
            //For Authentification !
            //var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = AdminId;
            var res = await _logsByDateBetweenClient.GetResponse<LogListResponse>(new { AdminId = userId, DateAfter = lower, DateBefore = upper });
            if (res == null) return StatusCode(500);
            return Ok(res);
        }

        [HttpGet("/logsByLevel/{level}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(List<LogEntry>))]
        public async Task<IActionResult> GetLogsByLevel(ELevel level)
        {
            //For Authentification !
            //var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = AdminId;
            var res = await _getLogsByLevelClient.GetResponse<LogListResponse>(new { AdminId = userId, Level = level });
            if (res == null) return StatusCode(500);
            return Ok(res);
        }
    }
}
