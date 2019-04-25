using ChatRoom.Api.Models;
using ChatRoom.Business.Interfaces;
using ChatRoom.Insfrastucture.Interfaces.Mappers;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatRoom.Api.Controllers
{
    public class ChatroomController : ApiController
    {
        private readonly IUserBusiness userBusiness;
        private readonly IChatroomBusiness chatroomBusiness;
        private readonly IMapper mapper;

        public ChatroomController(IUserBusiness userBusiness,
            IChatroomBusiness chatroomBusiness,
            IMapper mapper)
        {
            this.userBusiness = userBusiness;
            this.chatroomBusiness = chatroomBusiness;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a particular chatroom filtered by id
        /// </summary>
        /// <param name="id">Unique id per chatroom</param>
        /// <returns>Returns a single chatroom</returns>
        [HttpGet]
        [Route("~/api/chatrooms/{id}")]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                if (id <= 0) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Id {id} is not valid");
                var chatroomEntity = chatroomBusiness.GetById(id);
                if (chatroomEntity == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"Chatroom with id {id} does not exist.");
                var chatroomModel = mapper.MapFromEntityToModel(chatroomEntity);

                return Request.CreateResponse(HttpStatusCode.OK, chatroomModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Create a chatroom
        /// </summary>
        /// <param name="model">Chatroom creation object</param>
        /// <returns>Returns the created chatroom</returns>
        [HttpPost]
        [Route("~/api/chatrooms/")]
        public HttpResponseMessage Create([FromBody]ChatroomViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Owner)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid owner.");
                if (string.IsNullOrEmpty(model.Name)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Name is required");
                if (userBusiness.GetByNickName(model.Owner) == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"Owner with nickName {model.Owner} does not exist.");
                var chatroomEntity = mapper.MapFromModelToEntity(model);
                var chatroomId = chatroomBusiness.Create(chatroomEntity);
                chatroomEntity = chatroomBusiness.GetById(chatroomId);
                var chatroomModel = mapper.MapFromEntityToModel(chatroomEntity);
                return Request.CreateResponse(HttpStatusCode.Created, chatroomModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update the entire chatroom filtered by id
        /// </summary>
        /// <param name="model">Chatroom update object</param>
        /// <param name="id">Unique id per user. This will identify which chatroom will be updated</param>
        /// <returns>Returns the updated Chatroom</returns>
        [HttpPut]
        [Route("~/api/chatrooms/{id}")]
        public HttpResponseMessage Put([FromBody]ChatroomViewModel model, int id)
        {
            try
            {
                if (id <= 0) return Request.CreateResponse(HttpStatusCode.NotFound, "Invalid id");
                if (chatroomBusiness.GetById(id) == null) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Chatroom with id {id} does not exist.");
                if (string.IsNullOrEmpty(model.Name)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Name is required");
                if (userBusiness.GetByNickName(model.Owner) == null) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Owner with nickName {model.Owner} does not exist.");
                var entity = mapper.MapFromModelToEntity(model);
                chatroomBusiness.Update(entity);

                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete a chatroom filtered by id
        /// </summary>
        /// <param name="id">Unique id per chatroom. This will identify which chatroom will be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("~/api/chatrooms/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (id <= 0) return Request.CreateResponse(HttpStatusCode.NotFound, "Invalid id");
                if (chatroomBusiness.GetById(id) == null) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Chatroom with id {id} does not exist.");
                chatroomBusiness.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}