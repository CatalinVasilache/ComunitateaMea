using Microsoft.AspNetCore.Identity;
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
<<<<<<< HEAD
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
=======
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
>>>>>>> dc8d4b0d8f94addcb32e8b376cce49fde4af18fe
    }
}
