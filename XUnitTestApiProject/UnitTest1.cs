using AWSDemo.Controllers;
using AWSDemo.Helpers;
using AWSDemo.Interface;
using AWSDemo.Models;
using AWSDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestApiProject
{
    public class UnitTest1
    {

        private Mock<IAWSS3FileService> mockS3Service;
        private Mock<ILog> mockLogger;
        protected AWSS3FileController awss3Controller;

        [Fact]
        public void Test1()
        {
            Assert.Equal(1,1);
        }

        [Fact]
        public async void Task_GetPostByID_Return_Result()
        {
            mockS3Service =new Mock<IAWSS3FileService>();
            mockLogger = new Mock<ILog>();

            awss3Controller = new AWSS3FileController(mockS3Service.Object, mockLogger.Object);

            mockS3Service.Setup(m => m.GetFile(It.IsAny<string>()))
                .Returns(Task.FromResult(new Catalog
                {
                    Id = 1
                }
                ));

            //Act  
            var result = await awss3Controller.GetFile(1);

            if (result != null)
            {
                Assert.True(true, "Valid");
            }
            else
            {
                Assert.True(false, "InValid");

            }
        }


        [Fact]
        public async void Task_GetByAll_Return_Result()
        {
            mockS3Service = new Mock<IAWSS3FileService>();
            mockLogger = new Mock<ILog>();
            awss3Controller = new AWSS3FileController(mockS3Service.Object,mockLogger.Object);        
            var result = await awss3Controller.FilesListAsync();

            if (result != null)
            {
                Assert.True(true, "Valid");
            }
            else
            {
                Assert.True(false, "InValid");

            }
        }

        [Fact]
        public async void Task_UploadFile_Return_Result()
        {
            mockS3Service = new Mock<IAWSS3FileService>();
            mockLogger = new Mock<ILog>();
            awss3Controller = new AWSS3FileController(mockS3Service.Object, mockLogger.Object);

            mockS3Service.Setup(m => m.GetFile(It.IsAny<string>()))
               .Returns(Task.FromResult(new Catalog
               {
                   Name = "6"
               }
               ));


            var result = await awss3Controller.UploadFileAsync("6");

            if (result != null)
            {
                Assert.True(true, "Valid");
            }
            else
            {
                Assert.True(false, "InValid");

            }
        }

        [Fact]
        public async void Task_UpdateFile_Return_Result()
        {
            mockS3Service = new Mock<IAWSS3FileService>();
            mockLogger = new Mock<ILog>();
            awss3Controller = new AWSS3FileController(mockS3Service.Object, mockLogger.Object);

            mockS3Service.Setup(m => m.GetFile(It.IsAny<string>()))
                 .Returns(Task.FromResult(new Catalog
                 {
                     Name = "6"
                 }
                 ));


            var result = await awss3Controller.UpdateFile("5","6");

            if (result != null)
            {
                Assert.True(true, "Valid");
            }
            else
            {
                Assert.True(false, "InValid");
            }
        }

        [Fact]
        public async void Task_DeleteFile_Return_Result()
        {
            mockS3Service = new Mock<IAWSS3FileService>();
            mockLogger = new Mock<ILog>();
            awss3Controller = new AWSS3FileController(mockS3Service.Object, mockLogger.Object);

            mockS3Service.Setup(m => m.GetFile(It.IsAny<string>()))
            .Returns(Task.FromResult(new Catalog
            {
                Name = "6"
            }
            ));


            var result = await awss3Controller.DeleteFile("7");

            if (result != null)
            {
                Assert.True(true, "Valid");
            }
            else
            {
                Assert.True(false, "InValid");

            }
        }

        [Fact]
        public async void TestControllerCommunication()
        {

            var controller = new TestController();

            var result = controller.Test();

            if (result != null)
            {
                Assert.True(result == "API Workign fine", "Valid");
            }
            else
            {
                Assert.True(false, "InValid");

            }
        }

        [Fact]
        public async void NegativeTestCaseTask_GetPostByInvalidInput_Return_Result()
        {
            mockS3Service = new Mock<IAWSS3FileService>();
            mockLogger = new Mock<ILog>();

            awss3Controller = new AWSS3FileController(mockS3Service.Object, mockLogger.Object);

            mockS3Service.Setup(m => m.GetFile(It.IsAny<string>()))
                .Returns(Task.FromResult(new Catalog
                {
                    Id = 1
                }
                ));

            //Act  
            var result = await awss3Controller.GetFile(1000);


            var okResult = result as OkObjectResult;
           
            // assert
            Assert.Null(okResult);

        }

    }
}
