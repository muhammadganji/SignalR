using Microsoft.EntityFrameworkCore;
using WebAppSignalR.Contexts;
using WebAppSignalR.Models.Entities;

namespace WebAppSignalR.Models.Services
{
    public interface IChatRoomService
    {
        Task<Guid> CreateChatRoom(string ConnectionId);
        Task<Guid> GetChatRoomForConnection(string ConnectionId);
        Task<List<Guid>> GetAllRooms();
    }

    public class ChatRoomService : IChatRoomService
    {
        private readonly DatabaseContext _db;
        public ChatRoomService(DatabaseContext db)
        {
            _db = db;
        }
        public async Task<Guid> CreateChatRoom(string ConnectionId)
        {
            // جلوگیری از گروه تکراری
            var existChatRoom = _db.ChatRooms.SingleOrDefault(p => p.ConnectionId == ConnectionId);
            if (existChatRoom != null) {
                return await Task.FromResult(existChatRoom.Id);
            }

            ChatRoom chatRoom = new ChatRoom()
            {  
                ConnectionId = ConnectionId,
                Id = Guid.NewGuid(),
            };
            _db.ChatRooms.Add(chatRoom);
            _db.SaveChanges();
            return await Task.FromResult(chatRoom.Id);
        }

        

        public async Task<Guid> GetChatRoomForConnection(string ConnectionId)
        {
            var chatRoom = _db.ChatRooms.SingleOrDefault(p => p.ConnectionId == ConnectionId);
            return await Task.FromResult(chatRoom.Id);
        }



        public async Task<List<Guid>> GetAllRooms()
        {
            var rooms = _db.ChatRooms
                .Include(p => p.ChatMessages).Where(p => p.ChatMessages.Any()).Select(p => p.Id).ToList();
            return await Task.FromResult(rooms);
        }




    }
}
