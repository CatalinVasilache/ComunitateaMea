using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComunitateaMea.Models
{
    public class AppUser : IdentityUser
    {
        public override string Email { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public County County { get; set; }
    }
    public enum County
    {
        Ilfov,
        Galati,
        Braila,
        Brasov,
        Alba
    }
}
