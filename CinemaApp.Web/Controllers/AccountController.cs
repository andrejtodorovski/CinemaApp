using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Domain.Identity;
using CinemaApp.Repository;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CinemaApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CinemaAppUser> userManager;
        private readonly SignInManager<CinemaAppUser> signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<CinemaAppUser> userManager,
            SignInManager<CinemaAppUser> signInManager,ApplicationDbContext context)
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }
        // Со цел менаџирање на улогите на корисниците, потребно е да се изработи страница која ќе го овозможува тоа.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RoleManager()
        {
            return View(await _context.Users.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangeRole(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var role = await userManager.GetRolesAsync(user);
            if (role[0] == "User")
            {
                await userManager.RemoveFromRoleAsync(user, "User");
                await userManager.AddToRoleAsync(user, "Admin");
                user.Role = "Admin";
                await userManager.UpdateAsync(user);
                return RedirectToAction("RoleManager", "Account");
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, "Admin");
                await userManager.AddToRoleAsync(user, "User");
                user.Role = "User";
                await userManager.UpdateAsync(user);
                return RedirectToAction("RoleManager", "Account");
            }
            
        }
        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            UserRegistrationDto model = new UserRegistrationDto();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new CinemaAppUser
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Address = request.Address,
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserShoppingCart = new ShoppingCart(),
                        Role = "User"
                    };
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        // На почеток сите корисници се со улога „стандарден корисник“
                        await userManager.AddToRoleAsync(user, "User");
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }


        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, true, true);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    return RedirectToAction("Index", "Movies");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }

        [HttpPost]
        public async Task<IActionResult> ImportUsers(List<UserRegistrationDto> model)
        {
            var file = Request.Form.Files[0];
            if (file == null || file.Length == 0)
            {
                return RedirectToAction("RoleManager", "Account");
            }
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0).ToString() == "Email") continue;
                        var dto = new ImportUsersDTO
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Role = reader.GetValue(2).ToString()
                        };
                        var userCheck = userManager.FindByEmailAsync(dto.Email).Result;
                        if (userCheck == null)
                        {
                            var user = new CinemaAppUser
                            {
                                UserName = dto.Email.Split('@')[0],
                                Email = dto.Email,
                                EmailConfirmed = true,
                                PhoneNumberConfirmed = true,
                                Role = dto.Role,
                                UserShoppingCart = new ShoppingCart(),
                            };
                            var result = await userManager.CreateAsync(user, dto.Password);

                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(user, dto.Role);
                            }
                            else
                            {
                                if (result.Errors.Count() > 0)
                                {
                                    foreach (var error in result.Errors)
                                    {
                                        ModelState.AddModelError("message", error.Description);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("message", "Email already exists.");
                        }
                    }
                }
            }
            return RedirectToAction("RoleManager", "Account");

        }
    }
}
