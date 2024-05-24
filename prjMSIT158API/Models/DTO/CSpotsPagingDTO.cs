namespace prjMSIT158API.Models.DTO
{
    public class CSpotsPagingDTO
    {
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<SpotImagesSpot>? SpotsResult { get; set; }
    }
}
