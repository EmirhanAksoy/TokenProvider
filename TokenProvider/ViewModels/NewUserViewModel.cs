using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcelikAuthProvider.ViewModels
{
    public class NewUserViewModel
    {
        public bool IsActive { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
