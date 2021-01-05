using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcelikAuthProvider.Models
{
    public class Role : IdentityRole
    {
        public int IsletmeRefKey { get; set; }
    }
}
