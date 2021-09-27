using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using AWSDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AWSDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AWSBucketController : ControllerBase
    {
        S3Service _service = null;
        private readonly IAmazonS3 _client = null;

        [HttpGet]
        [Route("GetFile/{bucketName}")]
        public async Task<IActionResult> GetObjectFromS3Async(string bucketName)
        {
            new S3Service(_client);
            await _service.GetObjectFromS3Async(bucketName);
            return Ok();
        }
    }
}
