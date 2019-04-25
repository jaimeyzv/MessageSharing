using ChatRoom.Business.Entities;
using ChatRoom.Business.Interfaces;
using ChatRoom.DataAccess.Interfaces;
using ChatRoom.Insfrastucture.Interfaces.Mappers;
using System;

namespace ChatRoom.Business
{
    public class ChatroomBusiness : IChatroomBusiness
    {
        private readonly IChatroomRepository chatroomRepository;
        private readonly IMapper mapper;

        public ChatroomBusiness(IChatroomRepository chatroomRepository, IMapper mapper)
        {
            this.chatroomRepository = chatroomRepository;
            this.mapper = mapper;
        }

        public int Create(ChatroomEntity chatroom)
        {
            if (chatroom == null) throw new ArgumentNullException(nameof(chatroom));
            chatroom.IsActive = true;
            var dto = mapper.MapFromEntityToDto(chatroom);

            return Convert.ToInt32(chatroomRepository.Insert(dto));
        }

        public void Delete(int id)
        {
            var chatroomDto = chatroomRepository.GetById(id);
            if (chatroomDto != null)
                chatroomRepository.Delete(chatroomDto);
            else
                throw new ArgumentException($"Chatroom with id {id} does not exist.");
        }

        public ChatroomEntity GetById(int id)
        {
            var chatroomDto = chatroomRepository.GetById(id);
            return chatroomDto != null ? mapper.MapFromDtoToEntity(chatroomDto) : null;
        }

        public void Update(ChatroomEntity chatroom)
        {
            var chatroomDto = mapper.MapFromEntityToDto(chatroom);
            chatroomRepository.Update(chatroomDto);
        }
    }
}