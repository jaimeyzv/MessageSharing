using NPoco;

namespace ChatRoom.DataAccess.Dtos
{
    [TableName("User")]
    [PrimaryKey("UserId")]
    public class UserDto
    {
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string ProfileCode { get; set; }
    }
}