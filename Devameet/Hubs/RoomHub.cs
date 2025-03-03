using Devameet.Dtos;
using Devameet.Models;
using Devameet.Repository;
using Microsoft.AspNetCore.SignalR;

namespace Devameet.Hubs
{
    public class RoomHub : Hub
    {
        private readonly IRoomRepository _roomRepository;

        private string ClientId => Context.ConnectionId; // Criação do Id do cliente dentro do WebSocket (SignalR)

        public RoomHub(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public override async Task OnConnectedAsync() // Entrada do cliente no Hub com a geração do ClientId
        {
            Console.WriteLine("Client: " + ClientId + " está conectado.");
            await base.OnConnectedAsync();
        }

        public async Task Join(JoinDto joindto)
        {
            var link = joindto.Link; // Recupera link da sala
            var userid = joindto.UserId; // Recupera usuario

            Console.WriteLine("O usuário " + userid.ToString() + " está se juntando a sala com o CLientId: " + ClientId + " através do link " + link);

            await Groups.AddToGroupAsync(ClientId, link); // Coloca o ClientId dentro da sala correta.

            var updatePositionDto = new UpdatePositionDto(); //Determina a posição inicial do usuário
            updatePositionDto.X = 2; // Horizontal
            updatePositionDto.Y = 2; // Vertical
            updatePositionDto.Orientation = "down";

            await _roomRepository.UpdateUserPosition(userid, link, ClientId, updatePositionDto); // atualiza a posição do usuário e lista a posição de todos
            var users = await _roomRepository.ListUsersPosition(link);

            Console.WriteLine("Estamos enviando o novo cliente para os usuários: " + ClientId);
            await Clients.Group(link).SendAsync("update-user-list", new { Users = users }); // Mensagens para quem está na sala avisando que um novo usuario entrou.
            await Clients.OthersInGroup(link).SendAsync("add-user", new { User = ClientId });
            Console.WriteLine("Mensagens enviadas referente a entrada de um novo usuário.");
        }

        public async Task Move (MoveDto movedto)
        {
            var userId = Int32.Parse(movedto.UserId);
            var link = movedto.Link;

            var updatePositionDto = new UpdatePositionDto(); //Determina a posição inicial do usuário
            updatePositionDto.X = movedto.X; // Horizontal
            updatePositionDto.Y = movedto.Y; // Vertical
            updatePositionDto.Orientation = movedto.Orientation;

            await _roomRepository.UpdateUserPosition(userId, link, ClientId, updatePositionDto);
            var users = await _roomRepository.ListUsersPosition(link);
            Console.WriteLine("Estamos enviando a nova movimentação para todos os usuários.");
            await Clients.Group(link).SendAsync("update-user-list", new { Users = users });
        }

        public async Task UpdateUserMute(MuteDto mutedto)
        {
            var link = mutedto.Link;

            await _roomRepository.UpdateUserMute(mutedto);

            var users = await _roomRepository.ListUsersPosition(link);
            Console.WriteLine("Estamos enviando a nova movimentação para todos os usuários.");
            await Clients.Group(link).SendAsync("update-user-list", new { Users = users });

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine("Disconnecting client!");

            await _roomRepository.DeleteUserPosition(ClientId);
           
            await Clients.Others.SendAsync("remove - user", new { SocketId = ClientId});

            await base.OnDisconnectedAsync(exception);
            
        }

        public async Task CallUser(CallUserDto callUserDto)
        {
            await Clients.Client(callUserDto.To).SendAsync("call-made", new { Offer = callUserDto.Offer, Socket = ClientId });
        }

        public async Task MakeAnswer(MakeAnswerDto makeAnswerDto)
        {
            await Clients.Client(makeAnswerDto.To).SendAsync("Answer-made", new {Answer = makeAnswerDto.Answer, Socket = ClientId });  
        }

    }
}
