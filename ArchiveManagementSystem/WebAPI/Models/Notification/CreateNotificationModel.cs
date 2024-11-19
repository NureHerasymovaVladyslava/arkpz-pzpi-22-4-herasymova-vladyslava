namespace WebAPI.Models.Notification
{
    public class CreateNotificationModel
    {
        public int RoomId { get; set; }
        public int TypeId { get; set; }
        public string Text { get; set; }
    }
}
