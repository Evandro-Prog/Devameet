using Devameet.Dtos;
using Devameet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Devameet.Repository.Impl
{ 
    public class RoomRepositoryImpl : IRoomRepository
    {
        private readonly DevameetContext _context;

        public RoomRepositoryImpl(DevameetContext context)
        {
            _context = context;
        }

        public async Task DeleteUserPosition(string clientId)
        {
            var room = await _context.Rooms.Where(r =>  r.ClientId == clientId).FirstOrDefaultAsync();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<PositionDto>> ListUsersPosition(string link)
        {
            var meet = await _context.Meets.Where(m => m.Link == link).FirstOrDefaultAsync();
            var rooms = await _context.Rooms.Where(r => r.MeetId == meet.Id).ToListAsync();

            return rooms.Select(r => new PositionDto
            {
                X = r.X,
                Y = r.Y,
                Orientation = r.Orientation,
                Id = r.Id,
                Name = r.UserName,
                Avatar = r.Avatar,
                Muted = r.Muted,
                Meet = r.meet.Link,
                User = r.UserId.ToString(),
                ClientId = r.ClientId
            }).ToList();

        }

        public async Task UpdateUserMute(MuteDto mutedto)
        {
            var meet = await _context.Meets.Where(m => m.Link == mutedto.Link).FirstOrDefaultAsync();
            var room = await _context.Rooms.Where(r => r.MeetId == meet.Id && r.UserId == int.Parse(mutedto.UserId)).FirstOrDefaultAsync();

            room.Muted = mutedto.Muted;

            await _context.SaveChangesAsync();      
        }

        public async Task UpdateUserPosition(int userid, string link, string clientId, UpdatePositionDto updatePositionDto)
        {
            var meet = await _context.Meets.Where(m => m.Link == link).FirstOrDefaultAsync();
            var user = await _context.Users.Where(u => u.Id == userid).FirstOrDefaultAsync();

            var usersinroom = await _context.Rooms.Where(r => r.MeetId ==  meet.Id).ToListAsync();

            if(usersinroom.Count > 15)            
                throw new Exception("A sala já está cheia");

            if(usersinroom.Any(r => r.UserId == userid || r.ClientId == clientId))
            {
                var position = await _context.Rooms.Where(r => r.UserId == userid || r.ClientId == clientId).FirstOrDefaultAsync();
                position.Y = updatePositionDto.Y;
                position.X = updatePositionDto.X;
                position.Orientation = updatePositionDto.Orientation;
            }
            else
            {
                var room = new Room();
                room.Id = userid;
                room.ClientId = clientId;
                room.Y = updatePositionDto.Y;
                room.X = updatePositionDto.X;
                room.Orientation = updatePositionDto.Orientation;
                room.MeetId = meet.Id;
                room.UserName = user.Name;
                room.Avatar = user.Avatar;

                await _context.Rooms.AddAsync(room);

            }

            await _context.SaveChangesAsync();
            
        }
    }
}
