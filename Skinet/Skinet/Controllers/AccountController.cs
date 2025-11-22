using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skinet.DTOs;
using Skinet.Extensions;
using System.Security.Claims;

namespace Skinet.Controllers
{
    public class AccountController (SignInManager<AppUser> signInManager): BaseApiController
    {
        // We have alredy create out box from Ms identity the register,login, resfresh token,etc 
        //but if we want to customize we need to create the new ones
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result= await signInManager.UserManager.CreateAsync(user,registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return ValidationProblem();

        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }


        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false) return NoContent();

            var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);
              
            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                Address= user.Address?.ToDto()
            });
        }

        [HttpGet("auth-status")]
        public ActionResult GetAuthState()
        {

            return Ok(new
            {
                IsAuthenticated=User.Identity?.IsAuthenticated ?? false
            });
        }

        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto)
        {
            var user=await signInManager.UserManager.GetUserByEmailWithAddress(User);
            if (user is null)
            {
                user.Address=addressDto.ToEntity();
            }
            else
            {
                user.Address.UpdateFromDto(addressDto);
            }

            var result =await signInManager.UserManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest("Problem updating user address");
            return Ok(user.Address.ToDto());
        }


    }
}
