using System;

namespace WebApplication.DTO
{
    public class UserDto : IBaseEntity<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
