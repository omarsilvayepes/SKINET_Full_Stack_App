using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using System.Security.Claims;

namespace Skinet.Extensions
{
    public static class ClaimsPrincipleExtension
    {
        public static async Task<AppUser> GetUserByEmail(
            this UserManager<AppUser> manager, //(no a paramater) with this keyword add the GetUserByEmail  extension method to the userMnager class
            ClaimsPrincipal user)
        {
            var userToReturn=await manager.Users
                .FirstOrDefaultAsync(u=> u.Email==user.GetEmail());
            if (userToReturn is  null) throw new  AuthenticationException("User no found");
            return userToReturn;
        }

        public static async Task<AppUser> GetUserByEmailWithAddress(
            this UserManager<AppUser> manager, //(no a paramater) with this keyword add the GetUserByEmail  extension method to the userMnager class
            ClaimsPrincipal user)
        {
            var userToReturn = await manager.Users
                .Include(a => a.Address)
                .FirstOrDefaultAsync(u => u.Email == user.GetEmail());
            if (userToReturn is null) throw new AuthenticationException("User no found");
            return userToReturn;
        }

        public static string GetEmail(this ClaimsPrincipal user)  //(no a parameter)   this keyword to add to user the extention method Get Email
        {
            var email= user.FindFirstValue(ClaimTypes.Email) ?? throw new AuthenticationException("Email claim not found");
            return email;
        }
    }
}
