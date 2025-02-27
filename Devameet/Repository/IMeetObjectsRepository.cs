using Devameet.Models;

namespace Devameet.Repository
{
    public interface IMeetObjectsRepository
    {
        void CreateObjectsMeet(List<MeetObjects> meetObjectsNew, int meetId);
    }
}
