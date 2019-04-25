using System;

namespace ChatRoom.Common.QueueChatRoom
{
    public class QueueMessage
    {
        public string Menssge { get; set; }
        public DateTime Date { get; set; }
        public int ChatroomId { get; set; }
        public string UserSender { get; set; }
    }
}