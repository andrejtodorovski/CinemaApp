using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Domain.DTO
{
    public class ImportUsersDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
