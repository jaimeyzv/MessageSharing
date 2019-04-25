using NPoco;
using System;

namespace ChatRoom.DataAccess.Dtos
{
    [TableName("Message")]
    [PrimaryKey("MessageId")]
    public class MessageDto
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int ChatroomId { get; set; }
        public string UserSender { get; set; }
        public bool IsBot { get; set; }
    }
}