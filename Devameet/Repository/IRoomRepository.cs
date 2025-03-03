using Devameet.Dtos;

namespace Devameet.Repository
{
    public interface IRoomRepository
    {
        Task DeleteUserPosition(string clientId);
        Task <ICollection<PositionDto>>ListUsersPosition(string link);
        Task UpdateUserMute(MuteDto mutedto);
        Task UpdateUserPosition(int userid, string link, string clientId, UpdatePositionDto updatePositionDto);
    }
}
