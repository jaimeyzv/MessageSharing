namespace ChatRoom.Api.Models
{
    public class ChatroomViewModel
    {
        public int ChatroomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public bool IsActive { get; set; }
    }
}