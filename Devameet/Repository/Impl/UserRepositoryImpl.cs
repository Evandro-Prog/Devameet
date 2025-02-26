using Devameet.Models;

namespace Devameet.Repository.Impl
{
    public class UserRepositoryImpl : IUserRepository
    {
        private readonly DevameetContext _context;
        public UserRepositoryImpl(DevameetContext context) 
        { 
            _context = context;
        }

        public User GetUserByLogin(int iduser)
        {
            return _context.Users.FirstOrDefault(u => u.Id == iduser);

        }

        public User GetUserByLoginPassword(string login, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == login && u.Password == password);
        }

        public void Save(User user) // Adiciona usuario e salva no banco de dados
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public bool VerifyEmail(string email) // verifica se já existe o email cadastrado
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}
