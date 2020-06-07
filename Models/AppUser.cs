using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ComunitateaMea.Models
{
    public class AppUser : IdentityUser
    {
        public County County { get; set; }

        public int Age { get; set; }

        [Required]
        public string Salary { get; set; }
    }
    public enum County
    {
        Galati,
        Ilfov,
        Braila,
        Alba,
        Brasov
    }
}
