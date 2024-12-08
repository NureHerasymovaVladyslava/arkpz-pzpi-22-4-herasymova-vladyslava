using Core.Enums;

namespace WebAPI.Models.Document
{
    public class SearchFilterDocumentsModel
    {
        public string SearchQuery { get; set; } = ""
        public int? RoomId { get; set; }
        public int? StatusId { get; set; }
        public int? TypeId { get; set; }
        public OrderBy OrderBy { get; set; } = OrderBy.CreatedDesc;
    }
}
