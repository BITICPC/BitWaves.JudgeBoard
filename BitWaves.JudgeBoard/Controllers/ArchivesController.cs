using BitWaves.Data.Repositories;
using BitWaves.JudgeBoard.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace BitWaves.JudgeBoard.Controllers
{
    [ApiController]
    [Route("archives")]
    public sealed class ArchivesController : ControllerBase
    {
        private readonly Repository _repo;
        private readonly ILogger<ArchivesController> _logger;

        public ArchivesController(Repository repo, ILogger<ArchivesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET: /archives/{archiveId}
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(ObjectId archiveId)
        {
            _logger.LogInformation("Grabbing archive {0} for judge node {1}",
                                   archiveId, HttpContext.Connection.RemoteIpAddress);

            var (producer, consumer) = ProducerConsumerStream.Create();
            _repo.Problems.DownloadTestDataArchive(archiveId, producer).ConfigureAwait(false);
            return File(consumer, "application/zip");
        }
    }
}
