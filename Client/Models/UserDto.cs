using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    [Serializable]
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public ICollection<RoleDto> Roles { get; set; }
    }
}