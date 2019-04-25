using ChatRoom.Api.Models;
using ChatRoom.Business.Entities;
using ChatRoom.DataAccess.Dtos;
using ChatRoom.Insfrastucture.Interfaces.Mappers;

namespace ChatRoom.Insfrastucture.Mappers
{
    public class Mapper : IMapper
    {
        #region Profile

        public ProfileDto MapFromEntityToDto(ProfileEntity entity)
        {
            return new ProfileDto()
            {
                ProfileId = entity.ProfileId,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
        }

        public ProfileEntity MapFromDtoToEntity(ProfileDto dto)
        {
            return new ProfileEntity()
            {
                ProfileId = dto.ProfileId,
                Code = dto.Code,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
        }

        public ProfileEntity MapFromModelToEntity(ProfileViewModel model)
        {
            return new ProfileEntity()
            {
                ProfileId = model.ProfileId,
                Code = model.Code,
                Description = model.Description,
                IsActive = model.IsActive
            };
        }

        public ProfileViewModel MapFromEntityToModel(ProfileEntity entity)
        {
            return new ProfileViewModel()
            {
                ProfileId = entity.ProfileId,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
        }

        #endregion

        #region User


        public UserDto MapFromEntityToDto(UserEntity entity)
        {
            return new UserDto()
            {
                UserId = entity.UserId,
                NickName = entity.NickName,
                Name = entity.Name,
                LastName = entity.LastName,
                ProfileCode = entity.ProfileCode
            };
        }

        public UserEntity MapFromDtoToEntity(UserDto dto)
        {
            return new UserEntity()
            {
                UserId = dto.UserId,
                NickName = dto.NickName,
                Name = dto.Name,
                LastName = dto.LastName,
                ProfileCode = dto.ProfileCode
            };
        }

        public UserEntity MapFromModelToEntity(UserViewModel model)
        {
            return new UserEntity()
            {
                UserId = model.UserId,
                NickName = model.NickName,
                Name = model.Name,
                LastName = model.LastName,
                ProfileCode = model.ProfileCode
            };
        }

        public UserViewModel MapFromEntityToModel(UserEntity entity)
        {
            return new UserViewModel()
            {
                UserId = entity.UserId,
                NickName = entity.NickName,
                Name = entity.Name,
                LastName = entity.LastName,
                ProfileCode = entity.ProfileCode
            };
        }

        #endregion

        #region Chatroom

        public ChatroomDto MapFromEntityToDto(ChatroomEntity entity)
        {
            return new ChatroomDto()
            {
                ChatroomId = entity.ChatroomId,
                Name = entity.Name,
                Description = entity.Description,
                Owner = entity.Owner,
                IsActive = entity.IsActive
            };
        }

        public ChatroomEntity MapFromDtoToEntity(ChatroomDto dto)
        {
            return new ChatroomEntity()
            {
                ChatroomId = dto.ChatroomId,
                Name = dto.Name,
                Description = dto.Description,
                Owner = dto.Owner,
                IsActive = dto.IsActive
            };
        }

        public ChatroomEntity MapFromModelToEntity(ChatroomViewModel model)
        {
            return new ChatroomEntity()
            {
                ChatroomId = model.ChatroomId,
                Name = model.Name,
                Description = model.Description,
                Owner = model.Owner,
                IsActive = model.IsActive
            };
        }

        public ChatroomViewModel MapFromEntityToModel(ChatroomEntity entity)
        {
            return new ChatroomViewModel()
            {
                ChatroomId = entity.ChatroomId,
                Name = entity.Name,
                Description = entity.Description,
                Owner = entity.Owner,
                IsActive = entity.IsActive
            };
        }

        #endregion

        #region Message

        public MessageDto MapFromEntityToDto(MessageEntity entity)
        {
            return new MessageDto()
            {
                MessageId = entity.MessageId,
                Content = entity.Content,
                Date = entity.Date,
                ChatroomId = entity.ChatroomId,
                UserSender = entity.UserSender,
                IsBot = entity.IsBot
            };
        }

        public MessageEntity MapFromDtoToEntity(MessageDto dto)
        {
            return new MessageEntity()
            {
                MessageId = dto.MessageId,
                Content = dto.Content,
                Date = dto.Date,
                ChatroomId = dto.ChatroomId,
                UserSender = dto.UserSender,
                IsBot = dto.IsBot
            };
        }

        public MessageEntity MapFromModelToEntity(MessageViewModel model)
        {
            return new MessageEntity()
            {
                MessageId = model.MessageId,
                Content = model.Content,
                Date = model.Date,
                ChatroomId = model.ChatroomId,
                UserSender = model.UserSender,
                IsBot = model.IsBot
            };
        }

        public MessageViewModel MapFromEntityToModel(MessageEntity entity)
        {
            return new MessageViewModel()
            {
                MessageId = entity.MessageId,
                Content = entity.Content,
                Date = entity.Date,
                ChatroomId = entity.ChatroomId,
                UserSender = entity.UserSender,
                IsBot = entity.IsBot
            };
        }

        #endregion
    }
}