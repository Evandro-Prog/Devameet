using Devameet.Models;

namespace Devameet.Repository
{
    public interface IMeetRepository
    {
        void CreateMeet(Meet meet);
        void DeleteMeet(int meetid);
        Meet GetMeetsById(int meetId);
        List<Meet> GetMeetsByUser(int iduser);
        void UpdateMeet(Meet meet);
    }
}
