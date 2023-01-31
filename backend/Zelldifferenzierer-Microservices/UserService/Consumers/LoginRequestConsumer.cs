using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using UserServiceModels;
using UserServiceRequestMessages;
using UserServiceResponseMessages;

namespace UserService.Consumers
{
    public class LoginRequestConsumer : IConsumer<LoginRequest>
    {
        private readonly UserManager<ApplicationUser> _manager;
        
        public LoginRequestConsumer(UserManager<ApplicationUser> manager)
        {
            _manager = manager;

        }

        public async Task Consume(ConsumeContext<LoginRequest> context)
        {
            try
            {

                var user = await _manager.FindByEmailAsync(context.Message.Email);
                if (user != null  && !await _manager.IsLockedOutAsync(user))
                {
                    if (await _manager.CheckPasswordAsync(user, context.Message.Password))
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(Startup.Configuration["Secret"]);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {

                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.NameIdentifier, user.Id),
                                new Claim(ClaimTypes.Name, user.UserName),
                                new Claim(ClaimTypes.Role, user.Type.ToString())
                            }),
                            Expires = DateTime.UtcNow.AddDays(7),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        user.Token = tokenHandler.WriteToken(token);

                        await context.RespondAsync<LoginResponse>(new {Token = user.Token});
                    }

                    await _manager.AccessFailedAsync(user);
                    if (await _manager.IsLockedOutAsync(user))
                    {
                        Log.Information($"User had been locked out due to 5 failed login attempts {user.Email}");
                    }
                    Log.Information($"Login failed {context.Message.Email}");
                    await context.RespondAsync<LoginResponse>(new { });
                }
                else
                {
                    Log.Information($"Login failed used email address: {context.Message.Email}");
                    await context.RespondAsync<LoginResponse>(new { });
                }
            }
            catch (Exception e)
            {
                Log.Error($"LoginRequestConsumer threw an exception! Error message: {e}");
            }
           
        }
    }
}
