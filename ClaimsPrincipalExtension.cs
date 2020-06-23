using ComunitateaMea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ComunitateaMea.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        

        public static string GetFullName(this ClaimsPrincipal principal)
        {
            var fullName = principal.Claims.FirstOrDefault(c => c.Type == "FullName");
            return fullName?.Value;
        }
            
        public static string GetCounty(this ClaimsPrincipal principal)
        {
            var county = principal.Claims.Last(c => c.Type == "County");
            return county?.Value;
        }

        public static string GetAge(this ClaimsPrincipal principal)
        {
            var age = principal.Claims.FirstOrDefault(c => c.Type == "Age");
            return age?.Value;
        }
    }
}