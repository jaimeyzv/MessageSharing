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
    public class ProfileController : ApiController
    {
        private readonly IProfileBusiness profileBusiness;
        private readonly IMapper mapper;

        public ProfileController(IProfileBusiness profileBusiness,
            IMapper mapper)
        {
            this.profileBusiness = profileBusiness;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all profiles
        /// </summary>
        /// <returns>Returns a list of profiles</returns>
        [HttpGet]
        [Route("~/api/profiles/")]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var profileEntities = profileBusiness.GetAll().ToList();
                var ProfileViewModel = (from r in profileEntities select mapper.MapFromEntityToModel(r)).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, ProfileViewModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        /// <summary>
        /// Get a particular profile filtered by code
        /// </summary>
        /// <param name="code">Unique code per profile</param>
        /// <returns>Returns a single profile</returns>
        [HttpGet]
        [Route("~/api/profiles/{code}")]
        public HttpResponseMessage GetByCode(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid code");
                var profileEntity = profileBusiness.GetByCode(code);
                if (profileEntity == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"Profile with code {code} does not exist.");
                var ProfileViewModel = mapper.MapFromEntityToModel(profileEntity);

                return Request.CreateResponse(HttpStatusCode.OK, ProfileViewModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Create a new profile
        /// </summary>
        /// <param name="model">Profile creation object</param>
        /// <returns>Returns the created profile</returns>
        [HttpPost]
        [Route("~/api/profiles/")]
        public HttpResponseMessage Create([FromBody]ProfileViewModel model)
        {
            try
            {
                if (model == null) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid model");
                if (string.IsNullOrEmpty(model.Code)) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid profile code.");
                if (profileBusiness.GetByCode(model.Code) != null) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Profile code {model.Code} already exists.");
                var profileEntity = mapper.MapFromModelToEntity(model);
                profileBusiness.Create(profileEntity);
                var ProfileViewModel = profileBusiness.GetByCode(model.Code);
                return Request.CreateResponse(HttpStatusCode.Created, ProfileViewModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update the entire profile filtered by code
        /// </summary>
        /// <param name="model">Profile update object</param>
        /// <param name="code">Unique code per profile. This will identify which profile will be updated</param>
        /// <returns>Returns the updated profile</returns>
        [HttpPut]
        [Route("~/api/profiles/{code}")]
        public HttpResponseMessage Put([FromBody]ProfileViewModel model, [FromUri] string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid code");
                var exist = profileBusiness.GetByCode(code) != null;
                if (!exist) return Request.CreateResponse(HttpStatusCode.NotFound, $"Profile with code {code} does not exist.");
                model.Code = code;
                var entity = mapper.MapFromModelToEntity(model);
                profileBusiness.Update(entity);
                var ProfileViewModel = profileBusiness.GetByCode(model.Code);
                return Request.CreateResponse(HttpStatusCode.OK, ProfileViewModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete a profile filtered by code
        /// </summary>
        /// <param name="code">Unique code per profile. This will identify which profile will be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("~/api/profiles/{code}")]
        public HttpResponseMessage Delete(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid code");
                var exist = profileBusiness.GetByCode(code) != null;
                if (!exist) return Request.CreateResponse(HttpStatusCode.NotFound, $"Profile with code {code} does not exist.");
                profileBusiness.DeleteByCode(code);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}