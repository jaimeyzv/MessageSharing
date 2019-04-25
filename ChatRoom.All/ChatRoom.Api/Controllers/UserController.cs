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
    public class UserController : ApiController
    {
        private readonly IUserBusiness userBusiness;
        private readonly IProfileBusiness profileBusiness;
        private readonly IMapper mapper;

        public UserController(IUserBusiness userBusiness,
            IProfileBusiness profileBusiness,
            IMapper mapper)
        {
            this.userBusiness = userBusiness;
            this.profileBusiness = profileBusiness;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Returns a list of users</returns>
        [HttpGet]
        [Route("~/api/users/")]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var userEntities = userBusiness.GetAll().ToList();
                var userModels = (from r in userEntities select mapper.MapFromEntityToModel(r)).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, userModels);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get a particular user filtered by nickname
        /// </summary>
        /// <param name="nickName">Unique nickname per user</param>
        /// <returns>Returns a single user</returns>
        [HttpGet]
        [Route("~/api/users/{nickName}")]
        public HttpResponseMessage GetByNickName(string nickName)
        {
            try
            {
                if (string.IsNullOrEmpty(nickName)) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid nickName {nickName}");
                var userEntity = userBusiness.GetByNickName(nickName);
                if (userEntity == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"User with nickName {nickName} does not exist.");
                var userModel = mapper.MapFromEntityToModel(userEntity);

                return Request.CreateResponse(HttpStatusCode.OK, userModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="model">User creation object</param>
        /// <returns>Returns the created user</returns>
        [HttpPost]
        [Route("~/api/users/")]
        public HttpResponseMessage Create([FromBody]UserViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.NickName)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid nickName.");
                if (userBusiness.GetByNickName(model.NickName) != null) return Request.CreateResponse(HttpStatusCode.BadRequest, $"User with nickName {model.NickName} already exists.");
                if (string.IsNullOrEmpty(model.ProfileCode)) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid profile code.");
                if(profileBusiness.GetByCode(model.ProfileCode) == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"Profile code {model.ProfileCode} does not exist.");
                var userEntity = mapper.MapFromModelToEntity(model);
                userBusiness.Create(userEntity);
                var userModel = userBusiness.GetByNickName(model.NickName);
                return Request.CreateResponse(HttpStatusCode.Created, userModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update the entire user filtered by nickname
        /// </summary>
        /// <param name="model">User update object</param>
        /// <param name="nickName">Unique nickname per user. This will identify which user will be updated</param>
        /// <returns>Returns the updated user</returns>
        [HttpPut]
        [Route("~/api/users/{nickName}")]
        public HttpResponseMessage Put([FromBody]UserViewModel model, [FromUri] string nickName)
        {
            try
            {
                if (string.IsNullOrEmpty(nickName)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid nickName.");
                if (userBusiness.GetByNickName(nickName) == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"User with nickName {nickName} does not exist.");
                if (string.IsNullOrEmpty(model.ProfileCode)) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid profile code.");
                if (profileBusiness.GetByCode(model.ProfileCode) == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"Profile code {model.ProfileCode} does not exist.");
                model.NickName = nickName;
                var entity = mapper.MapFromModelToEntity(model);
                userBusiness.Update(entity);

                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete a user filtered by nickname
        /// </summary>
        /// <param name="nickName">Unique nickname per user. This will identify which user will be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("~/api/users/{nickName}")]
        public HttpResponseMessage Delete(string nickName)
        {
            try
            {
                if (string.IsNullOrEmpty(nickName)) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid code");
                var userEntity = userBusiness.GetByNickName(nickName);
                if (userEntity == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"User with nickName {nickName} does not exist.");
                userBusiness.Delete(userEntity.UserId);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}