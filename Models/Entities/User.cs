using System.ComponentModel.DataAnnotations;

namespace WebAppSignalR.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }
        [Display(Name ="رمز عبور")]
        public string Password { get; set; }
    }
}
