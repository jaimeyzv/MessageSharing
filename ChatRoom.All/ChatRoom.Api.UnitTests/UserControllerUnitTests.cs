using ChatRoom.Api.Controllers;
using ChatRoom.Api.Models;
using ChatRoom.Business.Entities;
using ChatRoom.Business.Interfaces;
using ChatRoom.Insfrastucture.Interfaces.Mappers;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatRoom.Api.UnitTests
{
    [TestFixture]
    public class UserControllerUnitTests
    {
        private Mock<IUserBusiness> userBusinessMock;
        private Mock<IProfileBusiness> profileBusinessMock;
        private Mock<IMapper> mapperMock;
        private UserController userController;

        [SetUp]
        public void SetUp()
        {
            userBusinessMock = new Mock<IUserBusiness>();
            profileBusinessMock = new Mock<IProfileBusiness>();
            mapperMock = new Mock<IMapper>();
            userController = new UserController(userBusinessMock.Object, profileBusinessMock.Object, mapperMock.Object);
        }

        [Test]
        public void GetAll_WhenRequestIsOk_ReturnsUserModelsAndOkStatus()
        {
            var fakeUserEntities = GetFakeUserEntities();
            userBusinessMock.Setup(x => x.GetAll()).Returns(fakeUserEntities);
            mapperMock.Setup(x => x.MapFromEntityToModel(It.IsAny<UserEntity>())).Returns(GetFakeUserModels()[0]);
            mapperMock.Setup(x => x.MapFromEntityToModel(It.IsAny<UserEntity>())).Returns(GetFakeUserModels()[1]);
            mapperMock.Setup(x => x.MapFromEntityToModel(It.IsAny<UserEntity>())).Returns(GetFakeUserModels()[2]);
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();

            var response = userController.GetAll();
            var responseResult = JsonConvert.DeserializeObject<List<UserViewModel>>(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(fakeUserEntities.Count, responseResult.Count);
        }

        [TestCase("jaimeyzv")]
        public void GetByNickName_WhenNickNameIsValid_ReturnsUserModel(string nickName)
        {
            var fakeUserEntity = GetFakeUserEntities().First(x => x.NickName == nickName);
            var fakeUserModel = GetFakeUserModels().First(x => x.NickName == nickName);
            userBusinessMock.Setup(x => x.GetByNickName(nickName)).Returns(fakeUserEntity);
            mapperMock.Setup(x => x.MapFromEntityToModel(It.IsAny<UserEntity>())).Returns(fakeUserModel);
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
            
            var response = userController.GetByNickName(nickName);
            var responseResult = JsonConvert.DeserializeObject<UserViewModel>(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(fakeUserEntity.NickName, responseResult.NickName);
            Assert.AreEqual(fakeUserEntity.Name, responseResult.Name);
            Assert.AreEqual(fakeUserEntity.LastName, responseResult.LastName);
        }

        [TestCase("")]
        [TestCase(null)]
        public void GetByNickName_WhenNickNameIsInvalid_ReturnsBadRequestStatus(string nickName)
        {
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
            
            var response = userController.GetByNickName(nickName);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [TestCase("merwin")]
        [TestCase("killer")]
        public void GetByNickName_WhenNickNameDoesNotExist_ReturnsNotFoundStatus(string nickName)
        {
            UserEntity userEntity = null;
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
            userBusinessMock.Setup(x => x.GetByNickName(nickName)).Returns(userEntity);

            var response = userController.GetByNickName(nickName);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Test]
        public void Create_WhenNickNameAlreadyExists_ReturnsBadRequestStatus()
        {
            var userEntity = GetFakeUserEntities()[0];
            var userModel = GetFakeUserModels()[0];
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
            userBusinessMock.Setup(x => x.GetByNickName(userEntity.NickName)).Returns(userEntity);

            var response = userController.Create(userModel);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test]
        public void Create_WhenProfileCodeDoesNotExist_ReturnsNotFoundStatus()
        {
            UserEntity userEntity = null;
            ProfileEntity profileEntity = null;
            var userModel = GetFakeUserModels()[0];
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
            userBusinessMock.Setup(x => x.GetByNickName(userModel.NickName)).Returns(userEntity);
            profileBusinessMock.Setup(x => x.GetByCode(userModel.ProfileCode)).Returns(profileEntity);

            var response = userController.Create(userModel);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Test]
        public void Create_WhenUserIsValid_ReturnsUserModelAndCreatedStatus()
        {
            UserEntity userEntityNull = null;
            var userEntity = GetFakeUserEntities().First(x => x.NickName == "jaimeyzv");
            var userModel = GetFakeUserModels().First(x => x.NickName == "jaimeyzv");

            userBusinessMock
                        .SetupSequence(x => x.GetByNickName(It.IsAny<string>()))
                        .Returns(userEntityNull)
                        .Returns(userEntity);
            profileBusinessMock.Setup(x => x.GetByCode(userModel.ProfileCode)).Returns(new ProfileEntity());
            mapperMock.Setup(x => x.MapFromModelToEntity(It.IsAny<UserViewModel>())).Returns(new UserEntity());
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();

            var response = userController.Create(userModel);
            var responseResult = JsonConvert.DeserializeObject<UserViewModel>(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
            Assert.AreEqual(userModel.NickName, responseResult.NickName);
            Assert.AreEqual(userModel.Name, responseResult.Name);
            Assert.AreEqual(userModel.LastName, responseResult.LastName);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Delete_WhenNickNameIsInvalid_ReturnsBadRequestStatus(string nickName)
        {
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();

            var response = userController.Delete(nickName);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [TestCase("merwin")]
        [TestCase("killer")]
        public void Delete_WhenNickNameDoesNotExist_ReturnsNotFoundStatus(string nickName)
        {
            UserEntity userEntity = null;
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
            userBusinessMock.Setup(x => x.GetByNickName(nickName)).Returns(userEntity);

            var response = userController.Delete(nickName);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        [TestCase("johao")]
        [TestCase("jaimeyzv")]
        public void Delete_WhenNickNameIsValid_ReturnsOkStatus(string nickName)
        {
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
            userBusinessMock.Setup(x => x.GetByNickName(nickName)).Returns(new UserEntity());

            var response = userController.Delete(nickName);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        private List<UserEntity> GetFakeUserEntities()
        {
            var users = new List<UserEntity>();
            users.Add(new UserEntity { Name = "Jaime", LastName = "Zamora", NickName = "jaimeyzv", ProfileCode = "ADMINISTRATOR" });
            users.Add(new UserEntity { Name = "Johao", LastName = "Rosas", NickName = "johao", ProfileCode = "USER" });
            users.Add(new UserEntity { Name = "Gustavo", LastName = "Guillen", NickName = "uriel4001", ProfileCode = "USER" });
            return users;
        }

        private List<UserViewModel> GetFakeUserModels()
        {
            var users = new List<UserViewModel>();
            users.Add(new UserViewModel { Name = "Jaime", LastName = "Zamora", NickName = "jaimeyzv", ProfileCode = "ADMINISTRATOR" });
            users.Add(new UserViewModel { Name = "Johao", LastName = "Rosas", NickName = "johao", ProfileCode = "USER" });
            users.Add(new UserViewModel { Name = "Gustavo", LastName = "Guillen", NickName = "uriel4001", ProfileCode = "USER" });
            return users;
        }
    }
}