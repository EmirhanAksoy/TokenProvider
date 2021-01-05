using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArcelikAuthProvider.Models
{
    public class User : IdentityUser
    {
        /// <summary>
        /// Not mapped.
        /// </summary>
        [NotMapped]
        public string Password { get; set; }


        /// <summary>
        /// Not mapped.
        /// </summary>
        [NotMapped]
        public IList<string> RoleNames { get; set; }

        public bool IsActive { get; set; }

    }
}
