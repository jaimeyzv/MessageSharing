using ChatRoom.Business.Entities;
using ChatRoom.Business.Interfaces;
using ChatRoom.DataAccess.Interfaces;
using ChatRoom.Insfrastucture.Interfaces.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserBusiness(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public int Create(UserEntity user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            var dto = mapper.MapFromEntityToDto(user);

            return Convert.ToInt32(userRepository.Insert(dto));
        }

        public void Delete(int id)
        {
            var user = userRepository.GetById(id);
            if (user != null)
                userRepository.Delete(user);
            else
                throw new ArgumentException($"User with id {id} does not exist.");
        }
        
        public List<UserEntity> GetAll()
        {
            var userDtos = userRepository.GetAll().ToList();
            var userEntities = (from r in userDtos select mapper.MapFromDtoToEntity(r));

            return userEntities.ToList();
        }

        public UserEntity GetById(int id)
        {
            var userDto = userRepository.GetById(id);
            var userEntity = mapper.MapFromDtoToEntity(userDto);

            return userEntity;
        }

        public UserEntity GetByNickName(string nickName)
        {
            var userDto = userRepository.GetByNickName(nickName);
            return userDto != null ? mapper.MapFromDtoToEntity(userDto) : null;
        }

        public void Update(UserEntity user)
        {
            var exists = userRepository.GetByNickName(user.NickName) != null;
            if (exists)
            {
                var userDto = mapper.MapFromEntityToDto(user);
                userRepository.Update(userDto);
            }
            else
                throw new ArgumentException($"User with NickName {user.NickName} does not exist.");
        }
    }
}
