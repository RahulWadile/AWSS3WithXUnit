using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWSDemo.Services
{
    public interface IS3Service
    {
        Task GetObjectFromS3Async(string bucketName);
    }
}
