using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Models;

namespace Blog.Extensions
{
    public static class RoleClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            // if(!user.Roles.Any())
            //     throw new ArgumentNullException("User have no roles");
            
            yield return new Claim(ClaimTypes.Name, user.Email);
            foreach (var role in user.Roles)
                yield return new Claim(ClaimTypes.Role, role.Slug);

                
        }
    }
}