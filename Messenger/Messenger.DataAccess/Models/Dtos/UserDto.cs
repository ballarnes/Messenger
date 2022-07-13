using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.DataAccess.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; } = null!;

        public string Email { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public bool isActivated { get; set; }

        public int EmailCode { get; set; }
    }
}
