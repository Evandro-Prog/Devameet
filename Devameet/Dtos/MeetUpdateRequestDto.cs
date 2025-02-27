namespace Devameet.Dtos
{
    public class MeetUpdateRequestDto
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public List<MeetObjectsDto> MeetObjects { get; set; }
    }
}
