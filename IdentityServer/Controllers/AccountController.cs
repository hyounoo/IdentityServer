using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityServer.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Controllers
{    
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> SignInManager;
        private readonly UserManager<IdentityUser> UserManager;
        private readonly ILogger<RegisterRequestModel> Logger;
        private readonly IEmailSender EmailSender;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<RegisterRequestModel> logger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Logger = logger;
        }

        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> Login([FromBody]LoginRequestModel model)
        //{
        //    //var aVal = 0; var blowUp = 1 / aVal;

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
            
        //    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
            
        //    if (result.Succeeded)
        //    {
        //        Logger.LogInformation("User logged in.");
        //        return Ok(result);
        //    }
        //    if (result.RequiresTwoFactor)
        //    {
        //        Logger.LogInformation("User account requires TwoFactor.");
        //    }
        //    if (result.IsLockedOut)
        //    {
        //        Logger.LogWarning("User account locked out.");                
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //    }

        //    return BadRequest(result);
        //}

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);


            Logger.LogInformation("User created a new account with password.");

            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code },
                protocol: Request.Scheme);

            await EmailSender.SendEmailAsync(model.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            await SignInManager.SignInAsync(user, isPersistent: false);


            return Ok(new RegisterResponseViewModel(user));
        }
    }
}