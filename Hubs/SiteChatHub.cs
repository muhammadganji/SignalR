using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebAppSignalR.Models.Services;

namespace WebAppSignalR.Hubs
{
    public class SiteChatHub: Hub
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IMessageService _messageService;
        public SiteChatHub(IChatRoomService chatRoomService, IMessageService messageService)
        {
            _chatRoomService = chatRoomService;
            _messageService = messageService;

        }

        /// <summary>
        /// پیوستن پشتیبان ها به گروه
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [Authorize]
        public async Task JoinRoom(Guid roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }


        /// <summary>
        /// ترک گروه توسط پشتیبان
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [Authorize]
        public async Task LeaveRoom(Guid roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }




        public async Task SendMessage(string Sender, string Message)
        {
            var roomId = await _chatRoomService.GetChatRoomForConnection(Context.ConnectionId);
            MessageDto messageDto = new MessageDto()
            {
                Message = Message,
                Sender = Sender,
                Time = DateTime.UtcNow.AddHours(3.5)
            };
            await _messageService.SaveChatMessage(roomId, messageDto);
            //await Clients.All.SendAsync("getNewMessage", Sender, Message, DateTime.UtcNow.AddHours(3.5).ToString("yyyy/MM/dd hh:mm"));
            await Clients.Group(roomId.ToString())
                .SendAsync("getNewMessage", messageDto.Sender, messageDto.Message, messageDto.Time.ToString("yyyy/MM/dd hh:mm"));
        }





        public override async Task OnConnectedAsync()
        {
            if(Context.User.Identity.IsAuthenticated)
            {
                await base.OnConnectedAsync();
                return;
            }
            var roomId = await _chatRoomService.CreateChatRoom(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            // اولین پیام که از طریق پشتیبانی دریافت میکنه
            await Clients.Caller.SendAsync("getNewMessage", "پشتیبانی", "سلام ارادت، چطور میتونم بهتون کمک کنم:"
                , DateTime.UtcNow.AddHours(3.5).ToString("yyyy/MM/dd hh:mm"));
            
            await base.OnConnectedAsync();
        }






        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
