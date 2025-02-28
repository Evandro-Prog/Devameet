using Devameet.Dtos;
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
            updatePositionDto.X = 2;
            updatePositionDto.Y = 2;
            updatePositionDto.Orientation = "down";

            await _roomRepository.UpdateUserPosition(userid, link, ClientId, updatePositionDto); // atualiza a posição do usuário e lista a posição de todos
            var users = await _roomRepository.ListUsersPosition(link);

            Console.WriteLine("Estamos enviando os usuários para atualização");

            await Clients.Group(link).SendAsync("update-user-list", new { Users = users }); // Mensagens para quem está na sala avisando que um novo usuario entrou.
            await Clients.OthersInGroup(link).SendAsync("add-user", new { User = ClientId });
        }

    }
}
