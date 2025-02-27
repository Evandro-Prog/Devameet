using Devameet.Models;

namespace Devameet.Repository
{
    public interface IMeetRepository
    {
        List<Meet> GetMeetsByUser(int iduser);
    }
}
