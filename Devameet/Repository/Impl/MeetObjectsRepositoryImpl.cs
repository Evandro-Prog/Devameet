using Devameet.Models;

namespace Devameet.Repository.Impl
{
    public class MeetObjectsRepositoryImpl : IMeetObjectsRepository
    {
        private readonly DevameetContext _context;

        public MeetObjectsRepositoryImpl(DevameetContext context)
        {
            _context = context;
        }
    }
}
