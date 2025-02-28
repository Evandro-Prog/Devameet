using Devameet.Dtos;

namespace Devameet.Repository
{
    public interface IRoomRepository
    {
        Task <ICollection<PositionDto>>ListUsersPosition(string link);
        Task UpdateUserPosition(int userid, string link, string clientId, UpdatePositionDto updatePositionDto);
    }
}
