namespace WebAPI.Models.AppUser
{
    public class CreateUserModel
    {
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public string EmailAddress { get; set; }
    }
}
