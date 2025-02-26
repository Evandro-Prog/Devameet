using Devameet.Models;

namespace Devameet.Repository
{
    public interface IUserRepository // Interfaces são as assinaturas dos metodos, usamos para que no construtor das classes ja consigamos estipular as assinaturas de metodos e usa-las de forma mais inteligente. 
    {
        User GetUserByLoginPassword(string login, string password);
        User GetUserByLogin(int iduser);
        void Save(User user);
        bool VerifyEmail(string email);
        void UpdateUser(User user);
    }
}
