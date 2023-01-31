using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HTTPRequestModels;
using LogServiceModels;
using LogServiceRequestMessages;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserServiceModels;
using UserServiceRequestMessages;
using UserServiceResponseMessages;

namespace GatewayService.Controllers
{

    /// GET = 200 OK, 404 NOT FOUND, 400 BAD REQUEST
    /// POST =  201 CREATED, 200 OK (if not able to reference by uri), 204 NO CONTENT
    /// PUT =  201 CREATED, 200 OK, 204 NO CONTENT, 404 NOT FOUND
    /// DELETE = 202 Accepted, 200 OK, 204 NO CONTENT, 404 NOT FOUND
    [Route("cellanalyzer/users")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IRequestClient<LoginRequest> _loginClient;
        private readonly IRequestClient<RegisterRequest> _registerClient;
        private readonly IRequestClient<GetAllUsersRequest> _getAllUsersClient;
        private readonly IRequestClient<DeleteUserRequest> _deleteUserRequestClient;
        private readonly IRequestClient<UpdateUserRequest> _updateUserRequestClient;
        private readonly IRequestClient<CreateAdminRequest> _registerAdminClient;
        private readonly IPublishEndpoint _endpoint;

        

        public UsersController(IRequestClient<LoginRequest> loginClient, IRequestClient<RegisterRequest> registerClient,
            IRequestClient<GetAllUsersRequest> getAllUsersClient, IRequestClient<DeleteUserRequest> deleteUserRequestClient,
            IRequestClient<UpdateUserRequest> updateUserRequestClient,
            IHttpContextAccessor accessor, IPublishEndpoint endpoint, IRequestClient<CreateAdminRequest> registerAdminClient)
        {
            _loginClient = loginClient;
            _registerClient = registerClient;
            _getAllUsersClient = getAllUsersClient;
            _deleteUserRequestClient = deleteUserRequestClient;
            _updateUserRequestClient = updateUserRequestClient;
            _accessor = accessor;
            _endpoint = endpoint;

            _registerAdminClient = registerAdminClient;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(200)]  
        [ProducesResponseType(401)] // Not Authorized
        public async Task<IActionResult> Login([FromBody] ClientLoginModel model)
        {
            var ip = _accessor.HttpContext.Connection.RemoteIpAddress;
            Serilog.Log.Information($"Login was requested by {ip}");

            var res = await _loginClient.GetResponse<LoginResponse>(new {Email = model.Email, Password = model.Password});

            if (res.Message.Token == null)
            {
                await _endpoint.Publish<LogRequest>(new {LogMessage = $"Unsuccessful login attempt of {model.Email} with IP {ip}",
                    Sender = "GatewayService/UserController", Level = ELevel.Information});

                return StatusCode(401);
            }

            await _endpoint.Publish<LogRequest>(new
            {
                LogMessage = $"Successful login attempt of {model.Email} with IP {ip}",
                Sender = "GatewayService/UserController",
                Level = ELevel.Information
            });
            return Ok(JsonConvert.SerializeObject(res.Message.Token));

        }

        [HttpPost("registerNewUser")]
        [Authorize(Policy = "AdministratorsOnly")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)] 
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterModel model)
        {
            var currentUser = new CurrentUser(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    HttpContext.User.FindFirst(ClaimTypes.Name).Value);

           Serilog.Log.Information($"RegisterNewUser was requested by {currentUser}");

            var res = await _registerClient.GetResponse<RegisterResponse>(new {
                Title = model.Title, 
                Firstname = model.Firstname,
                Lastname =  model.Lastname,
                IdentificationNumber = model.IdentificationNumber,
                Email = model.Email,
                Password = model.Password});

            return StatusCode(res.Message.Success ? 200 : 400);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)] 
        [Authorize(Policy = "AdministratorsOnly")]
        public async Task<IActionResult> GetAllUser()
        {
            var currentUser = new CurrentUser(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, HttpContext.User.FindFirst(ClaimTypes.Name).Value);
            Serilog.Log.Information($"GetAllUsers was requested by {currentUser}");

            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var res = await _getAllUsersClient.GetResponse<GetAllUsersResponse>(new {AdminId = userId});
            if (res.Message.Users != null) return Ok(res);
            return StatusCode(500);
        }


        
        [HttpDelete("deleteUser/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [Authorize(Policy = "AdministratorsOnly")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var currentUser = new CurrentUser(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, HttpContext.User.FindFirst(ClaimTypes.Name).Value);
            Serilog.Log.Information($"DeleteUser was requested by {currentUser}");

            var res = await _deleteUserRequestClient.GetResponse<SuccessResponse>(new { UserId  = id});
            if (res.Message.Success)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPut("updateCurrentUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] ApplicationUser user)
        {
            var currentUser = new CurrentUser(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, HttpContext.User.FindFirst(ClaimTypes.Name).Value);
            Serilog.Log.Information($"UpdateCurrentUser was requested by {currentUser}");

            return Ok();  
        }

        [HttpPut("updateUser/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [Authorize(Policy = "AdministratorsOnly")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] ApplicationUser user)
        {
            var currentUser = new CurrentUser(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, HttpContext.User.FindFirst(ClaimTypes.Name).Value);
            Serilog.Log.Information($"UpdateUser was requested by {currentUser}");

            return Ok();
        }

        [HttpPost("registerAdmin/{type}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)] 
        [Authorize(Policy = "AdministratorsOnly")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model, UserType type)
        {
            var currentUser = new CurrentUser(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, HttpContext.User.FindFirst(ClaimTypes.Name).Value);
            Serilog.Log.Information($"RegisterNewUser was requested by {currentUser}");

            var res = await _registerAdminClient.GetResponse<RegisterResponse>(new
            {
                Title = model.Title,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                IdentificationNumber = model.IdentificationNumber,
                Email = model.Email,
                Password = model.Password,
                UserType = type
            });

            return StatusCode(res.Message.Success ? 200 : 400);
        }
    }
}
