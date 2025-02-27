

namespace Devameet.Models
{
    public class MeetObjects
    {
        public int Id { get; set; }
        public int MeetId { get; set; }
        public string Name { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int ZIndex { get; set; } // indicie de prioridade
        public string Orientation { get; set; }
    }
}
