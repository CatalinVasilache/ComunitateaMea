using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ComunitateaMea.Data;
using ComunitateaMea.Extensions;
using ComunitateaMea.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ComunitateaMea.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "County")]
            public County County { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //var county = User.GetCounty();

            Username = userName;
            //Enum.TryParse(county, out County county1);
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                County = user.County
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var User = await _userManager.GetUserAsync(base.User);
            if (User == null)
            {
                return base.NotFound($"Unable to load user with ID '{_userManager.GetUserId(base.User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(User);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(User);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(User, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            //var county = User.GetCounty();
            //if (Input.County.ToString() != county)
            //{
            //    user.County = Input.County;
            //    await _userManager.UpdateAsync(user);
            //    await _context.SaveChangesAsync();
            //    System.Diagnostics.Debug.WriteLine("new county", user);
            //}
            System.Diagnostics.Debug.WriteLine("old county", User.County);
            if (Input.County != User.County)
            {
                User.County = Input.County;
            }
            await _userManager.AddClaimAsync(User, new Claim("County", User.County.ToString()));
            await _userManager.UpdateAsync(User);
            _context.Update(User);
            await _context.SaveChangesAsync();
            System.Diagnostics.Debug.WriteLine("new county", User.County);
            await _signInManager.RefreshSignInAsync(User);
            //await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            ModelState.Clear();
            return base.RedirectToPage(User);
        }
    }
}
