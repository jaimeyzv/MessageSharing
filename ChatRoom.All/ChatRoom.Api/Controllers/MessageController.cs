using ChatRoom.Api.Models;
using ChatRoom.Business.Interfaces;
using ChatRoom.Insfrastucture.Interfaces.Mappers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatRoom.Api.Controllers
{
    public class MessageController : ApiController
    {
        private readonly IUserBusiness userBusiness;
        private readonly IMessageBusiness messageBusiness;
        private readonly IChatroomBusiness chatroomBusiness;
        private readonly IMapper mapper;

        public MessageController(IUserBusiness userBusiness,
            IMessageBusiness messageBusiness,
            IChatroomBusiness chatroomBusiness,
            IMapper mapper)
        {
            this.userBusiness = userBusiness;
            this.messageBusiness = messageBusiness;
            this.chatroomBusiness = chatroomBusiness;
            this.mapper = mapper;
        }

        /// <summary>
        /// Create a message
        /// </summary>
        /// <param name="model">Message creation object</param>
        /// <returns>Returns the created message</returns>
        [HttpPost]
        [Route("~/api/messages/")]
        public HttpResponseMessage Create([FromBody]MessageViewModel model)
        {
            try
            {
                if(model.ChatroomId <= 0) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid chatroom id.");
                if (chatroomBusiness.GetById(model.ChatroomId) == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"Chatroom with id {model.ChatroomId} does not exist.");
                if (string.IsNullOrEmpty(model.UserSender)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid user sender.");
                if (userBusiness.GetByNickName(model.UserSender) == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"User sender {model.UserSender} does not exist.");
                model.Date = DateTime.Now;
                var messageEntity = mapper.MapFromModelToEntity(model);
                var messageId = messageBusiness.Create(messageEntity);
                if(messageId == 2) return Request.CreateResponse(HttpStatusCode.OK);
                messageEntity = messageBusiness.GetById(messageId);
                var messageModel = mapper.MapFromEntityToModel(messageEntity);
                return Request.CreateResponse(HttpStatusCode.Created, messageModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get 50 messages that belong to a particular chatroom
        /// </summary>
        /// <param name="chatroomId">Unique id that identity a chatroom</param>
        /// <returns>Return a list of messages</returns>
        [HttpGet]
        [Route("~/api/messages/{chatroomId}")]
        public HttpResponseMessage Get(int chatroomId)
        {
            try
            {
                if (chatroomId <= 0) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid chatroom id.");
                if (chatroomBusiness.GetById(chatroomId) == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"Chatroom with id {chatroomId} does not exist.");
                var messageEntities = messageBusiness.GetLatest50MessagesByChatroomId(chatroomId);
                var messageModels = (from r in messageEntities select mapper.MapFromEntityToModel(r));
                
                return Request.CreateResponse(HttpStatusCode.Created, messageModels.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get all messages that belong to a particular chatroom
        /// </summary>
        /// <param name="chatroomId">Unique id that identity a chatroom</param>
        /// <returns>Return a list of messages</returns>
        [HttpGet]
        [Route("~/api/messages/{chatroomId}/history")]
        public HttpResponseMessage GetHistory(int chatroomId)
        {
            try
            {
                if (chatroomId <= 0) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid chatroom id.");
                if (chatroomBusiness.GetById(chatroomId) == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"Chatroom with id {chatroomId} does not exist.");
                var messageEntities = messageBusiness.GetAllMessagesByChatroomId(chatroomId);
                var messageModels = (from r in messageEntities select mapper.MapFromEntityToModel(r));

                return Request.CreateResponse(HttpStatusCode.Created, messageModels.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}