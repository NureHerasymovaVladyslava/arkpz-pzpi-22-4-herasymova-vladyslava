namespace WebAPI.Models.Document
{
    public class EditDocumentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
