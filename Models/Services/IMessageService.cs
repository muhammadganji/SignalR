using WebAppSignalR.Contexts;
using WebAppSignalR.Models.Entities;

namespace WebAppSignalR.Models.Services
{
    public interface IMessageService
    {
        Task SaveChatMessage(Guid RoomId, MessageDto Message);
        Task<List<MessageDto>> GetChatMessages(Guid RoomId);
    }

    public class MessageService : IMessageService
    {
        private readonly DatabaseContext _db;
        public MessageService(DatabaseContext db)
        {
            _db = db;
        }
        public Task<List<MessageDto>> GetChatMessages(Guid RoomId)
        {
            var messages = _db.ChatMessages.Where(p => p.ChatRoomId == RoomId)
                .Select(p => new MessageDto
                {
                    Message = p.Message,
                    Sender = p.Sender,
                    Time = p.Time,
                })
                .OrderBy(p => p.Time).ToList();
            return Task.FromResult(messages);
        }

        public Task SaveChatMessage(Guid RoomId, MessageDto Message)
        {
            var room = _db.ChatRooms.SingleOrDefault(p => p.Id == RoomId);
            ChatMessage chatMessage = new ChatMessage()
            {
                ChatRoom = room,
                Message = Message.Message,
                Sender = Message.Sender,
                Time = Message.Time,
            };
            _db.ChatMessages.Add(chatMessage);
            _db.SaveChanges();
            return Task.CompletedTask;
        }
    }

    public class MessageDto
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
