namespace WebAPI.Models.AppUser
{
    public class EditUserModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public string EmailAddress { get; set; }
    }
}
