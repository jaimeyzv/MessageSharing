namespace ChatRoom.Api.Models
{
    public class ProfileViewModel
    {
        public int ProfileId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}