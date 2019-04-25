using ChatRoom.Business.Entities;
using ChatRoom.Business.Interfaces;
using ChatRoom.Common.QueueChatRoom;
using ChatRoom.DataAccess.Interfaces;
using ChatRoom.Insfrastucture.Interfaces.Mappers;
using ChatRoom.MessageBroker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatRoom.Business
{
    public class MessageBusiness : IMessageBusiness
    {
        private readonly IMessageRepository messageRepository;
        private readonly IQueueHandler queueHandler;
        private readonly IMapper mapper;

        public MessageBusiness(IMessageRepository messageRepository, IQueueHandler queueHandler, IMapper mapper)
        {
            this.messageRepository = messageRepository;
            this.queueHandler = queueHandler;
            this.mapper = mapper;
        }

        public int Create(MessageEntity message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (UseQueue(message.Content))
            {
                var queueMessage = new QueueMessage();
                queueMessage.Date = message.Date;
                queueMessage.Menssge = message.Content;
                queueMessage.ChatroomId = message.ChatroomId;
                queueMessage.UserSender = message.UserSender;

                queueHandler.SendMessage("ChatroomDemo", queueMessage);
                return 2;
            }
            message.IsBot = false;
            var dto = mapper.MapFromEntityToDto(message);
            return Convert.ToInt32(messageRepository.Insert(dto));
        }

        public void CreateMessageFromWorker(MessageEntity message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));            
            var dto = mapper.MapFromEntityToDto(message);
            messageRepository.Insert(dto);
        }

        public List<MessageEntity> GetAllMessagesByChatroomId(int chatroomId)
        {
            var messageDtos = messageRepository.GetAllMessagesByChatroomId(chatroomId).ToList();
            var messageEntities = (from r in messageDtos select mapper.MapFromDtoToEntity(r));

            return messageEntities.ToList();
        }

        public MessageEntity GetById(int id)
        {
            var messageDto = messageRepository.GetById(id);
            var messageEntity = mapper.MapFromDtoToEntity(messageDto);

            return messageEntity;
        }

        public List<MessageEntity> GetLatest50MessagesByChatroomId(int chatroomId)
        {
            var messageDtos = messageRepository.GetLatest50MessagesByChatroomId(chatroomId).ToList();
            var messageEntities = (from r in messageDtos select mapper.MapFromDtoToEntity(r));

            return messageEntities.ToList();
        }

        private bool UseQueue(string message)
        {
            var commands = new List<string>() { "/stock=APPL", "/day_range=APPL" };
            return commands.Any(x => x == message);
        }
    }
}