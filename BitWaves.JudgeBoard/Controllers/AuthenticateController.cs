using System;
using System.Threading.Tasks;
using AutoMapper;
using BitWaves.JudgeBoard.Models;
using BitWaves.JudgeBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BitWaves.JudgeBoard.Controllers
{
    [ApiController]
    [Route("auth")]
    public sealed class AuthenticateController : ControllerBase
    {
        private readonly IJudgeAuthenticationService _authService;
        private readonly IJwtIssuer _jwtIssuer;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticateController> _logger;

        public AuthenticateController(IJudgeAuthenticationService authService,
                                      IJwtIssuer jwtIssuer,
                                      IMapper mapper,
                                      ILogger<AuthenticateController> logger)
        {
            _authService = authService;
            _jwtIssuer = jwtIssuer;
            _mapper = mapper;
            _logger = logger;
        }

        // POST: /auth
        [HttpPost]
        public async Task<ActionResult<AuthenticationSessionModel>> Authenticate()
        {
            var addr = HttpContext.Connection.RemoteIpAddress;
            _logger.LogInformation("Authenticating remote address {}", addr);

            var session = await _authService.CreateSessionAsync();
            return _mapper.Map<JudgeAuthenticationSession, AuthenticationSessionModel>(session);
        }

        // PATCH: /auth/{sessionID}
        [HttpPatch("{sessionId}")]
        public async Task<ActionResult<ChallengeResultModel>> Challenge(
            string submissionId,
            [FromBody] byte[] challenge)
        {
            if (!Guid.TryParse(submissionId, out var id))
            {
                ModelState.AddModelError(nameof(submissionId), "Invalid session ID.");
                return ValidationProblem();
            }

            var succeed = await _authService.ChallengeAsync(id, challenge);
            if (!succeed)
            {
                return Challenge();
            }

            var jwt = await _jwtIssuer.IssueAsync(HttpContext.Connection.RemoteIpAddress.ToString());
            return new ChallengeResultModel
            {
                Jwt = jwt
            };
        }
    }
}
