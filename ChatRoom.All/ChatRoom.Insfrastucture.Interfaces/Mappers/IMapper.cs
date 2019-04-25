using ChatRoom.Api.Models;
using ChatRoom.Business.Entities;
using ChatRoom.DataAccess.Dtos;

namespace ChatRoom.Insfrastucture.Interfaces.Mappers
{
    public interface IMapper
    {
        #region Profile

        ProfileDto MapFromEntityToDto(ProfileEntity entity);
        ProfileEntity MapFromDtoToEntity(ProfileDto dto);
        ProfileEntity MapFromModelToEntity(ProfileViewModel model);
        ProfileViewModel MapFromEntityToModel(ProfileEntity entity);

        #endregion

        #region User

        UserDto MapFromEntityToDto(UserEntity entity);
        UserEntity MapFromDtoToEntity(UserDto dto);
        UserEntity MapFromModelToEntity(UserViewModel model);
        UserViewModel MapFromEntityToModel(UserEntity entity);

        #endregion

        #region Chatroom

        ChatroomDto MapFromEntityToDto(ChatroomEntity entity);
        ChatroomEntity MapFromDtoToEntity(ChatroomDto dto);
        ChatroomEntity MapFromModelToEntity(ChatroomViewModel model);
        ChatroomViewModel MapFromEntityToModel(ChatroomEntity entity);

        #endregion

        #region Message

        MessageDto MapFromEntityToDto(MessageEntity entity);
        MessageEntity MapFromDtoToEntity(MessageDto dto);
        MessageEntity MapFromModelToEntity(MessageViewModel model);
        MessageViewModel MapFromEntityToModel(MessageEntity entity);

        #endregion
    }
}