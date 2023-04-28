using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebAppSignalR.Models.Services;

namespace WebAppSignalR.Hubs
{
    [Authorize]
    public class SupportHub: Hub
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IMessageService _messageService;
        private readonly IHubContext<SiteChatHub> _siteChatHub;
        public SupportHub(IChatRoomService chatRoomService, IMessageService messageService, IHubContext<SiteChatHub> siteChatHub)
        {
            _chatRoomService = chatRoomService;
            _messageService = messageService;
            _siteChatHub = siteChatHub;

        }


        public async Task LoadMessages(Guid roomId)
        {
            var messages = await _messageService.GetChatMessages(roomId);
            await Clients.Caller.SendAsync("getNewMessage", messages);
        }

        // ارسال پیام از طریق پنل
        public async Task SendMessage(Guid roomId, string message)
        {
            MessageDto messageDto = new MessageDto()
            {
                Sender = Context.User.Identity.Name,
                Message = message,
                Time = DateTime.UtcNow.AddHours(3.5)
            };
            await _messageService.SaveChatMessage(roomId, messageDto);

            await _siteChatHub.Clients.Group(roomId.ToString())
                .SendAsync("getNewMessage", messageDto.Sender, messageDto.Message, messageDto.Time.ToString("yyyy/MM/dd hh:mm"));
        }



        public async override Task OnConnectedAsync()
        {
            var rooms = await _chatRoomService.GetAllRooms();
            await Clients.Caller.SendAsync("GetRooms", rooms);
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


    }
}
