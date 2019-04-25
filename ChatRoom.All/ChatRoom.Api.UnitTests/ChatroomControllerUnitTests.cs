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
    public class ChatroomControllerUnitTests
    {
        private Mock<IUserBusiness> userBusinessMock;
        private Mock<IChatroomBusiness> chatroomBusiness;
        private Mock<IMapper> mapperMock;
        private ChatroomController chatroomController;

        [SetUp]
        public void SetUp()
        {
            userBusinessMock = new Mock<IUserBusiness>();
            chatroomBusiness = new Mock<IChatroomBusiness>();
            mapperMock = new Mock<IMapper>();
            chatroomController = new ChatroomController(userBusinessMock.Object, chatroomBusiness.Object, mapperMock.Object);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void GetById_WhenIdIsInvalid_ReturnsBadRequestStatus(int id)
        {
            chatroomController.Request = new HttpRequestMessage();
            chatroomController.Configuration = new HttpConfiguration();

            var response = chatroomController.GetById(id);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);
        }
        
        [TestCase(123)]
        public void GetById_WhenIdDoesNotExists_ReturnsNotFoundStatus(int id)
        {
            ChatroomEntity chatRoomEntity = null;
            chatroomBusiness.Setup(x => x.GetById(id)).Returns(chatRoomEntity);
            chatroomController.Request = new HttpRequestMessage();
            chatroomController.Configuration = new HttpConfiguration();

            var response = chatroomController.GetById(id);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetById_WhenIdIsValid_ReturnsBadRequestStatus(int id)
        {
            var fakeChatroomEntity = GetFakeChatroomEntities().First(x => x.ChatroomId == id);
            var fakeChatroomModel = GetFakeChatroomModels().First(x => x.ChatroomId == id);
            chatroomBusiness.Setup(x => x.GetById(id)).Returns(fakeChatroomEntity);
            mapperMock.Setup(x => x.MapFromEntityToModel(It.IsAny<ChatroomEntity>())).Returns(fakeChatroomModel);
            chatroomController.Request = new HttpRequestMessage();
            chatroomController.Configuration = new HttpConfiguration();

            var response = chatroomController.GetById(id);
            var responseResult = JsonConvert.DeserializeObject<ChatroomViewModel>(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(fakeChatroomEntity.ChatroomId, responseResult.ChatroomId);
            Assert.AreEqual(fakeChatroomEntity.Name, responseResult.Name);
            Assert.AreEqual(fakeChatroomEntity.Description, responseResult.Description);
            Assert.AreEqual(fakeChatroomEntity.Owner, responseResult.Owner);
            Assert.AreEqual(fakeChatroomEntity.IsActive, responseResult.IsActive);
        }

        [TestCase("jaimeyzv")]
        [TestCase("johao")]
        public void Create_WhenOwnerIsInvalid_ReturnsNotFoundStatus(string owner)
        {
            UserEntity userEntityNull = null;
            var fakeChatroomModel = GetFakeChatroomModels().First(x => x.Owner == owner);
            userBusinessMock.Setup(x=>x.GetByNickName(owner)).Returns(userEntityNull);
            chatroomController.Request = new HttpRequestMessage();
            chatroomController.Configuration = new HttpConfiguration();

            var response = chatroomController.Create(fakeChatroomModel);
            
            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }
        
        public void Create_WhenModelIsValid_ReturnsChatroomModelAndCreatedStatus()
        {
            var fakeChatroomEntity = GetFakeChatroomEntities().First(x => x.ChatroomId == 1);
            var fakeChatroomModel = GetFakeChatroomModels().First(x => x.ChatroomId == 1);
            userBusinessMock.Setup(x => x.GetByNickName(fakeChatroomModel.Owner)).Returns(new UserEntity());
            mapperMock.Setup(x => x.MapFromEntityToModel(It.IsAny<ChatroomEntity>())).Returns(fakeChatroomModel);
            chatroomController.Request = new HttpRequestMessage();
            chatroomController.Configuration = new HttpConfiguration();

            var response = chatroomController.Create(fakeChatroomModel);
            var responseResult = JsonConvert.DeserializeObject<ChatroomViewModel>(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);
            Assert.AreEqual(fakeChatroomEntity.ChatroomId, responseResult.ChatroomId);
            Assert.AreEqual(fakeChatroomEntity.Name, responseResult.Name);
            Assert.AreEqual(fakeChatroomEntity.Description, responseResult.Description);
            Assert.AreEqual(fakeChatroomEntity.Owner, responseResult.Owner);
            Assert.AreEqual(fakeChatroomEntity.IsActive, responseResult.IsActive);
        }

        private List<ChatroomEntity> GetFakeChatroomEntities()
        {
            var chatrooms = new List<ChatroomEntity>();
            chatrooms.Add(new ChatroomEntity() { ChatroomId = 1, Name = ".Net Core Web Api", Description = "Any description text", Owner = "jaimeyzv", IsActive = true });
            chatrooms.Add(new ChatroomEntity() { ChatroomId = 2, Name = ".Net Web Api", Description = "Any description text", Owner = "johao", IsActive = true });
            return chatrooms;
        }

        private List<ChatroomViewModel> GetFakeChatroomModels()
        {
            var chatrooms = new List<ChatroomViewModel>();
            chatrooms.Add(new ChatroomViewModel() { ChatroomId = 1, Name = ".Net Core Web Api", Description = "Any description text", Owner = "jaimeyzv", IsActive = true });
            chatrooms.Add(new ChatroomViewModel() { ChatroomId = 2, Name = ".Net Web Api", Description = "Any description text", Owner = "johao", IsActive = true });
            return chatrooms;
        }
    }
}