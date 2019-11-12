using Distvisor.Web.Data;
using Distvisor.Web.Data.Models;
using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;
        private readonly DistvisorContext _distvisorContext;

        public AuthenticationController(
            ICryptoService cryptoService,
            DistvisorContext distvisorContext)
        {
            _cryptoService = cryptoService;
            _distvisorContext = distvisorContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok($"Hello {User.Identity.Name}");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // create user if there is no user at all
            if (!_distvisorContext.Users.Any())
            {
                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Username = login.Username,
                    PasswordHash = _cryptoService.GeneratePasswordHash(login.Password),
                    LockoutUtc = DateTime.UtcNow,
                };
                _distvisorContext.Add(newUser);
                _distvisorContext.SaveChanges();
            }

            // verify credentials
            var user = _distvisorContext.Users.FirstOrDefault(x => x.Username == login.Username);
            if (user == null)
                return Unauthorized();

            var lockoutSeconds = (int)(user.LockoutUtc - DateTime.UtcNow).TotalSeconds - 50;
            if (lockoutSeconds > 0)
                return Unauthorized($"Lockout for {lockoutSeconds} seconds.");

            var authenticated = (_cryptoService.ValidatePasswordHash(login.Password, user.PasswordHash));
            if (!authenticated)
            {
                user.LockoutUtc = user.LockoutUtc > DateTime.UtcNow ? user.LockoutUtc.AddSeconds(10) : DateTime.UtcNow.AddSeconds(10);
                _distvisorContext.SaveChanges();
                return Unauthorized();
            }

            // generate user session
            var session = new Session
            {
                Id = _cryptoService.GenerateRandomSessionId(),
                IssuedAtUtc = DateTime.UtcNow,
                ExpireOnUtc = DateTime.Now.AddDays(30),
                User = user,
            };
            _distvisorContext.Add(session);
            _distvisorContext.SaveChanges();

            return Ok(new LoginSuccessResponseDto
            {
                Username = user.Username,
                SessionId = session.Id,
                SessionIssueDate = session.IssuedAtUtc,
                SessionExpirationDate = session.ExpireOnUtc,
            });
        }
    }

    public class LoginRequestDto
    {
        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }

    public class LoginSuccessResponseDto
    {
        public string Username { get; set; }
        public string SessionId { get; set; }
        public DateTime SessionIssueDate { get; set; }
        public DateTime SessionExpirationDate { get; set; }
    }
}
