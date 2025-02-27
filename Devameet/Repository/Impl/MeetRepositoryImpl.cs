using Devameet.Models;

namespace Devameet.Repository.Impl
{
    public class MeetRepositoryImpl : IMeetRepository
    {
        private readonly DevameetContext _context;

        public MeetRepositoryImpl(DevameetContext context)
        {
            _context = context;
        }

        public void CreateMeet(Meet meet)
        {
            _context.Meets.Add(meet);
            _context.SaveChanges();
        }

        public void DeleteMeet(int meetid)
        {
            List<MeetObjects> meetObjectsExist = _context.MeetObjects.Where(o => o.MeetId == meetid).ToList();
            foreach (MeetObjects meetObj in meetObjectsExist) //Percorre lista de objetos na meet e deleta todos antes de excluir a sala
            {
                _context.MeetObjects.Remove(meetObj);
                _context.SaveChanges();
            }

            Meet meet = _context.Meets.FirstOrDefault(m => m.Id == meetid); // Após a exculsao dos objetos deleta a sala
            _context.Remove(meet);
            _context.SaveChanges();
        }

        public Meet GetMeetsById(int meetId)
        {
            return _context.Meets.FirstOrDefault(m => m.Id == meetId);
        }

        public List<Meet> GetMeetsByUser(int iduser)
        {
            return _context.Meets.Where(u => u.Id == iduser).ToList();
        }

        public void UpdateMeet(Meet meet)
        {
            _context.Meets.Update(meet);
            _context.SaveChanges();
        }
    }
}
