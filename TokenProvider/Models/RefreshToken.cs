using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArcelikAuthProvider.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }

        public DateTime Created { get; set; }

    }
}
