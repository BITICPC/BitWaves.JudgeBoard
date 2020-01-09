using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using BitWaves.JudgeBoard.Models;
using BitWaves.JudgeBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BitWaves.JudgeBoard.Controllers
{
    [ApiController]
    [Route("judges")]
    public sealed class JudgesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IJudgeNodeManager _judgeNodeManager;
        private readonly ILogger<JudgesController> _logger;

        public JudgesController(IMapper mapper, IJudgeNodeManager judgeNodeManager, ILogger<JudgesController> logger)
        {
            _mapper = mapper;
            _judgeNodeManager = judgeNodeManager;
            _logger = logger;
        }

        // GET: /judges
        [HttpGet]
        public async Task<ActionResult<List<JudgeNodeInfoModel>>> Get()
        {
            var nodes = await _judgeNodeManager.GetAllAsync();
            return nodes.Select(i => _mapper.Map<JudgeNodeInfo, JudgeNodeInfoModel>(i))
                        .ToList();
        }

        // PATCH: /judges
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Patch(
            [FromBody] PatchJudgeNodeInfoModel model)
        {
            _logger.LogInformation("Heartbeat packet received from {0}", HttpContext.Connection.RemoteIpAddress);

            var performance = _mapper.Map<PatchJudgeNodeInfoModel, JudgeNodePerformanceInfo>(model);
            await _judgeNodeManager.UpdatePerformanceAsync(HttpContext.Connection.RemoteIpAddress, performance);
            return Ok();
        }

        // PUT: /judges/{address}/block
        [HttpPut("{address}/block")]
        public async Task<IActionResult> PutBlock(
            string address,
            [FromQuery(Name = "blocked")] bool blocked = true)
        {
            if (!IPAddress.TryParse(address, out var ipAddr))
            {
                ModelState.AddModelError(nameof(address), "invalid IP address.");
                return ValidationProblem();
            }

            var ret = await _judgeNodeManager.SetBlockedAsync(ipAddr, blocked);
            if (!ret)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
