using ChatRoom.Business.Entities;
using ChatRoom.Business.Interfaces;
using ChatRoom.DataAccess.Interfaces;
using ChatRoom.Insfrastucture.Interfaces.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatRoom.Business
{
    public class ProfileBusiness : IProfileBusiness
    {
        private readonly IProfileRepository profileRepository;
        private readonly IMapper mapper;

        public ProfileBusiness(IProfileRepository profileRepository, IMapper mapper)
        {
            this.profileRepository = profileRepository;
            this.mapper = mapper;
        }

        public int Create(ProfileEntity profile)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            profile.IsActive = true;
            var dto = mapper.MapFromEntityToDto(profile);

            return Convert.ToInt32(profileRepository.Insert(dto));
        }

        public void Delete(int id)
        {
            var profile = profileRepository.GetById(id);
            if (profile != null)
                profileRepository.Delete(profile);
            else
                throw new ArgumentException($"Profile with id {id} does not exist.");
        }

        public void DeleteByCode(string code)
        {
            var profile = profileRepository.GetByCode(code);
            if (profile != null)
                profileRepository.Delete(profile);
            else
                throw new ArgumentException($"Profile with code {code} does not exist.");
        }

        public List<ProfileEntity> GetAll()
        {
            var profileDtos = profileRepository.GetAll().ToList();
            var profileEntities = (from r in profileDtos select mapper.MapFromDtoToEntity(r));

            return profileEntities.ToList();
        }

        public ProfileEntity GetByCode(string code)
        {
            var profileDto = profileRepository.GetByCode(code);
            if (profileDto != null)
            {
                var profileEntity = mapper.MapFromDtoToEntity(profileDto);
                return profileEntity;
            }

            return null;
        }
        
        public void Update(ProfileEntity profile)
        {
            var profileEntity = GetByCode(profile.Code);
            if (profileEntity != null)
            {
                var profileDto = mapper.MapFromEntityToDto(profile);
                profileDto.ProfileId = profileEntity.ProfileId;
                profileRepository.Update(profileDto);
            }
            else
                throw new ArgumentException($"Profile with id {profile.ProfileId} does not exist.");
        }
    }
}