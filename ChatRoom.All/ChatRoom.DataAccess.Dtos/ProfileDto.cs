using NPoco;

namespace ChatRoom.DataAccess.Dtos
{
    [TableName("Profile")]
    [PrimaryKey("ProfileId")]
    public class ProfileDto
    {
        public int ProfileId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}