namespace prjMSIT158API.Models.DTO
{
    public class CSearchDTO
    {
        public int categoryId { get; set; } = 0;
        public string? keyword { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 9;
        public string? sortType { get; set; }
        public string? sortBy { get; set; }

    }
}
