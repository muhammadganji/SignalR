using Microsoft.EntityFrameworkCore;
using WebAppSignalR.Models.Entities;

namespace WebAppSignalR.Contexts
{
    public class DatabaseContext: DbContext
    {
        //private readonly string connectionString = "Data source=.; Initial Catalog=WebAppSignalR_DB_2023; User Id= mamad; Password=10203040;Trusted_connection=true; trustServerCertificate=true;";

        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options) {
        }

        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
