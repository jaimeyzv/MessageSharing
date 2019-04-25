using System;

namespace ChatRoom.Business.Entities
{
    public class MessageEntity
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int ChatroomId { get; set; }
        public string UserSender { get; set; }
        public bool IsBot { get; set; }
    }
}