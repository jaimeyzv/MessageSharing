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
    public class ProfileControllerUnitTests
    {
        private Mock<IProfileBusiness> profileBusinessMock;
        private Mock<IMapper> mapperMock;
        private ProfileController profileController;

        [SetUp]
        public void SetUp()
        {
            profileBusinessMock = new Mock<IProfileBusiness>();
            mapperMock = new Mock<IMapper>();
            profileController = new ProfileController(profileBusinessMock.Object, mapperMock.Object);
        }

        [Test]
        public void GetAll_WhenRequestIsOk_ReturnsProfileModelsAndOkStatus()
        {
            var fakeProfileEntities = GetFakeProfileEntities();
            profileBusinessMock.Setup(x => x.GetAll()).Returns(fakeProfileEntities);
            mapperMock.Setup(x => x.MapFromEntityToModel(It.IsAny<ProfileEntity>())).Returns(GetFakeProfileModels()[0]);
            mapperMock.Setup(x => x.MapFromEntityToModel(It.IsAny<ProfileEntity>())).Returns(GetFakeProfileModels()[1]);

            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();

            var response = profileController.GetAll();
            var responseResult = JsonConvert.DeserializeObject < List <ProfileViewModel>>(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(fakeProfileEntities.Count, responseResult.Count);
        }

        [TestCase("ADMINISTRATOR")]
        public void GetByCode_WhenProfileCodeIsValid_ReturnsProfileModel(string code)
        {
            var fakeProfileEntity = GetFakeProfileEntities().First(x => x.Code == code);
            var fakeProfileModel = GetFakeProfileModels().First(x => x.Code == code);
            profileBusinessMock.Setup(x => x.GetByCode(code)).Returns(fakeProfileEntity);
            mapperMock.Setup(x => x.MapFromEntityToModel(It.IsAny<ProfileEntity>())).Returns(fakeProfileModel);
            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();

            var response = profileController.GetByCode(code);
            var responseResult = JsonConvert.DeserializeObject<ProfileViewModel>(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(fakeProfileEntity.Code, responseResult.Code);
            Assert.AreEqual(fakeProfileEntity.Description, responseResult.Description);
            Assert.AreEqual(fakeProfileEntity.IsActive, responseResult.IsActive);
        }

        [TestCase("")]
        [TestCase(null)]
        public void GetByCode_WhenProfileCodeIsInvalid_ReturnsProfileModel(string code)
        {
            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();

            var response = profileController.GetByCode(code);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [TestCase("ADMIN")]
        [TestCase("USERS")]
        public void GetByCode_WhenProfileCodeDoesNotExist_ReturnsNotFoundStatus(string code)
        {
            ProfileEntity profileEntity = null;
            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();
            profileBusinessMock.Setup(x => x.GetByCode(code)).Returns(profileEntity);

            var response = profileController.GetByCode(code);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Test]
        public void Create_WhenProfileCodeAlreadyExists_ReturnsBadRequestStatus()
        {
            var profileEntity = GetFakeProfileEntities()[0];
            var profileModel = GetFakeProfileModels()[0];
            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();
            profileBusinessMock.Setup(x => x.GetByCode(profileEntity.Code)).Returns(profileEntity);

            var response = profileController.Create(profileModel);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test]
        public void Create_WhenUserIsValid_ReturnsUserModelAndCreatedStatus()
        {
            ProfileEntity profileEntityNull = null;
            var profileEntity = GetFakeProfileEntities().First(x => x.Code == "ADMINISTRATOR");
            var profileModel = GetFakeProfileModels().First(x => x.Code == "ADMINISTRATOR");

            profileBusinessMock
                        .SetupSequence(x => x.GetByCode(It.IsAny<string>()))
                        .Returns(profileEntityNull)
                        .Returns(profileEntity);
            mapperMock.Setup(x => x.MapFromModelToEntity(It.IsAny<UserViewModel>())).Returns(new UserEntity());
            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();

            var response = profileController.Create(profileModel);
            var responseResult = JsonConvert.DeserializeObject<ProfileViewModel>(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
            Assert.AreEqual(profileModel.Code, responseResult.Code);
            Assert.AreEqual(profileModel.Description, responseResult.Description);
            Assert.AreEqual(profileModel.IsActive, responseResult.IsActive);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Put_WhenProfileCodeIsInvalid_ReturnsBadRequestStatus(string code)
        {
            var profileEntity = GetFakeProfileEntities()[0];
            var profileModel = GetFakeProfileModels()[0];
            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();
            profileBusinessMock.Setup(x => x.GetByCode(profileEntity.Code)).Returns(profileEntity);

            var response = profileController.Put(profileModel, code);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [TestCase("ADMINISTRATOR")]
        public void Patch_WhenProfileIsValid_ReturnsProfileModelAndOkStatus(string code)
        {
            var profileEntity = GetFakeProfileEntities().First(x => x.Code == code);
            var profileModel = GetFakeProfileModels().First(x => x.Code == code);

            profileBusinessMock
                        .SetupSequence(x => x.GetByCode(It.IsAny<string>()))
                        .Returns(profileEntity);
            mapperMock.Setup(x => x.MapFromModelToEntity(It.IsAny<ProfileViewModel>())).Returns(profileEntity);
            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();

            var response = profileController.Put(profileModel, code);
            var responseResult = JsonConvert.DeserializeObject<ProfileViewModel>(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(profileModel.Code, responseResult.Code);
            Assert.AreEqual(profileModel.Description, responseResult.Description);
            Assert.AreEqual(profileModel.IsActive, responseResult.IsActive);
        }

        [TestCase("ADMIN")]
        [TestCase("USERS")]
        public void Delete_WhenProfilCodeDoesNotExist_ReturnsNotFoundStatus(string code)
        {
            ProfileEntity profileEntityNull = null;
            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();
            profileBusinessMock.Setup(x => x.GetByCode(code)).Returns(profileEntityNull);

            var response = profileController.Delete(code);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        [TestCase("ADMINISTRATOR")]
        [TestCase("USER")]
        public void Delete_WhenProfileCodeIsValid_ReturnsOkStatus(string code)
        {
            profileController.Request = new HttpRequestMessage();
            profileController.Configuration = new HttpConfiguration();
            profileBusinessMock.Setup(x => x.GetByCode(code)).Returns(new ProfileEntity());

            var response = profileController.Delete(code);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
        
        private List<ProfileEntity> GetFakeProfileEntities()
        {
            var profiles = new List<ProfileEntity>();
            profiles.Add(new ProfileEntity() { Code = "ADMINISTRATOR", Description = "Any Text", IsActive = true });
            profiles.Add(new ProfileEntity() { Code = "USER", Description = "Any Text", IsActive = true });
            return profiles;
        }

        private List<ProfileViewModel> GetFakeProfileModels()
        {
            var profiles = new List<ProfileViewModel>();
            profiles.Add(new ProfileViewModel() { Code = "ADMINISTRATOR", Description = "Any Text", IsActive = true });
            profiles.Add(new ProfileViewModel() { Code = "USER", Description = "Any Text", IsActive = true });
            return profiles;
        }
    }
}