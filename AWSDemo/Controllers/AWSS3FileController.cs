using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using AWSDemo.Interface;
using AWSDemo.Models;
using AWSDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static AWSDemo.Models.EnumModel;

namespace AWSDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AWSS3FileController : ControllerBase
    {
        private readonly IAWSS3FileService _AWSS3FileService;
        private ILog logger;
        public AWSS3FileController(IAWSS3FileService AWSS3FileService, ILog logger)
        {
            this._AWSS3FileService = AWSS3FileService;
            this.logger = logger;
        }

        //[Route("ReadFile/{fileName}")]
        //[HttpGet]
        //public async Task<IActionResult> ReadObjectDataAsync(string fileName)
        //{           
        //    var result = await _AWSS3FileService.ReadfileContent(fileName);

        //    return Content(result, "application/json");
        //   // return Ok(result);
        //}

        [Route("GetFile/{CatlogID}")]
        [HttpGet]
        //public async Task<Catalog> GetFile(int? CatlogID)
        public async Task<IActionResult> GetFile(int? CatlogID)
        {
            try
            {
                logger.Information("Get File Method called");
                logger.Warning("Warning is logged");
                logger.Debug("Debug log is logged");

                if (CatlogID != 0 || string.IsNullOrEmpty(Convert.ToString(CatlogID)))
                {
                    logger.Warning("Provide the valid CatlogID");
                    return NotFound("Provide the valid CatlogID");
                }

                var result = await _AWSS3FileService.GetFile(CatlogID.ToString());
                if (result == null)
                {
                    logger.Warning("Product not found");
                    return NotFound("Product not found");
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                logger.Error("Error is logged" + ex.InnerException);

                return BadRequest("Error in execution");
            }

        }

        [Route("FilesList")]
        [HttpGet]
        // public async Task<IEnumerable<Catalog>> FilesListAsync()
        public async Task<IActionResult> FilesListAsync()
        {
            try
            {
                logger.Information("FilesList Method called");

                var result = await _AWSS3FileService.FilesList();
                if (result == null)
                {
                    logger.Warning("Product not found");
                    return NotFound("Product not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error("Error is logged" + ex.InnerException);
                return BadRequest("Error in execution FilesList");
            }
            //return await _AWSS3FileService.FilesList();

        }

        [Route("UploadFile/{fileName}")]
        [HttpPost]
        public async Task<IActionResult> UploadFileAsync(string fileName)
        {
            try
            {
                logger.Information("UploadFile Method called");

                if (string.IsNullOrEmpty(Convert.ToString(fileName)))
                {
                    logger.Warning("Provide the valid File Name");
                    return NotFound("Provide the valid File Name");
                }

                var result = await _AWSS3FileService.UploadFile(fileName);

                if (result == false)
                {
                    logger.Warning("File not upload");
                    return NotFound("File not upload");
                }
                return Ok(new { isSucess = result });
            }
            catch (Exception ex)
            {
                logger.Error("Error is logged" + ex.InnerException);
                return BadRequest("Error in execution UploadFile");
            }

        }

        [Route("UpdateFile")]
        [HttpPut]
        public async Task<IActionResult> UpdateFile(string Id, string Name)
        {
            try
            {
                logger.Information("UpdateFile Method called");
                Id = Id.Trim();
                if (string.IsNullOrEmpty(Convert.ToString(Name)) && string.IsNullOrEmpty(Convert.ToString(Id)))
                {
                    logger.Warning("Provide the valid File Name and Upload File Name");
                    return NotFound("Provide the valid Name and Catlog ID");
                }

                if (string.IsNullOrEmpty(Convert.ToString(Name)))
                {
                    logger.Warning("Provide the valid  Name");
                    return NotFound("Provide the valid Name");
                }

                if (string.IsNullOrEmpty(Convert.ToString(Id)) || Id == "0")
                {
                    logger.Warning("Provide the valid Catlog ID");
                    return NotFound("Provide the valid Catlog ID");
                }

                string filePath = Convert.ToString(Id) + ".json";


                Catalog inst = new Catalog();
                inst.Id = Convert.ToInt32(Id);
                inst.Name = Name;


                var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
                GetObjectRequest request = new GetObjectRequest();


                request.BucketName = "catlog-bucket"; // "sateeshmphasisbucket1";
                request.Key = filePath;



                GetObjectResponse response = await client.GetObjectAsync(request);


                StreamReader reader = new StreamReader(response.ResponseStream);


                string content = reader.ReadToEnd();
                List<Catalog> dese = JsonConvert.DeserializeObject<List<Catalog>>(content);
                int catID = Convert.ToInt32(Id);
                dese.RemoveAll(x => x.Id == catID);
                dese.Add(inst);
                //string serializedval =JsonSerializer. .ser(dese);
                string serializedval = JsonConvert.SerializeObject(dese);


                System.IO.File.WriteAllText(filePath, string.Empty);
                System.IO.File.WriteAllText(filePath, serializedval);



                var result = await _AWSS3FileService.UpdateFile(filePath, filePath);
                if (result == false)
                {
                    logger.Warning("Product not found");
                    return NotFound("Product not found");
                }

                return Ok(new { isSucess = result });
            }
            catch (Exception ex)
            {
                logger.Error("Error is logged" + ex.InnerException);
                return BadRequest("Error in execution UpdateFile");
            }

        }
        [Route("DeleteFile/{fileName}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            try
            {
                logger.Information("FilesList Method called DeleteFile");

                if (string.IsNullOrEmpty(Convert.ToString(fileName)))
                {
                    logger.Warning("Provide the valid File Name");
                    return NotFound("Provide the valid File Name");
                }


                var result = await _AWSS3FileService.DeleteFile(fileName);

                if (result == false)
                {
                    logger.Warning("Product not found");
                    return NotFound("Product not found");
                }
                return Ok(new { isSucess = result });

            }
            catch (Exception ex)
            {
                logger.Error("Error is logged" + ex.InnerException);
                return BadRequest("Error in execution DeleteFile");
            }

        }

    }
}
