using System.Threading.Tasks;
using AutoMapper;
using BitWaves.Data.Entities;
using BitWaves.Data.Repositories;
using BitWaves.JudgeBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace BitWaves.JudgeBoard.Controllers
{
    [ApiController]
    [Route("submissions")]
    public sealed class SubmissionsController : ControllerBase
    {
        private readonly Repository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<SubmissionsController> _logger;

        public SubmissionsController(Repository repo, IMapper mapper, ILogger<SubmissionsController> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: /submissions
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<SubmissionModel>> Get()
        {
            _logger.LogInformation("Fetching submission info for judge node {0}",
                                   HttpContext.Connection.RemoteIpAddress);

            while (true)
            {
                var submission = await _repo.Submissions.FindOneUnjudgedSubmissionAsync();
                if (submission == null)
                {
                    return NoContent();
                }

                var archiveId = await _repo.Problems.GetProblemTestDataArchiveIdAsync(submission.ProblemId);
                if (archiveId == null)
                {
                    // No available test data archive. The verdict of the submission should be directly set to
                    // Verdict.NoTestData.
                    _logger.LogWarning("No available test data archive found for submission: {0}, problem: {}",
                                       submission.Id, submission.ProblemId);

                    var judgeResult = new JudgeResult
                    {
                        Verdict = Verdict.NoTestData
                    };
                    await _repo.Submissions.SetJudgeResultAsync(submission.Id, judgeResult);

                    continue;
                }

                var model = _mapper.Map<Submission, SubmissionModel>(submission);
                model.ArchiveId = archiveId.Value;

                return model;
            }
        }

        // PATCH: /submissions/{submissionId}
        [HttpPatch("{submissionId}")]
        public async Task<IActionResult> Patch(
            ObjectId submissionId,
            [FromBody] SubmissionJudgeResultModel model)
        {
            _logger.LogInformation("Judge of submission {0} finished by node {1}",
                                   submissionId, HttpContext.Connection.RemoteIpAddress);

            var judgeResult = _mapper.Map<SubmissionJudgeResultModel, JudgeResult>(model);

            await _repo.Submissions.SetJudgeResultAsync(submissionId, judgeResult);
            return Ok();
        }
    }
}
