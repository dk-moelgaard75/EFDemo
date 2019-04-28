using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EFDemo.Models;
using EFDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace EFDemo.Controllers
{
    public class UserController : Controller
    {
        private IUserService _iUserService;
        public UserController(IUserService service)
        {
            _iUserService = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserModel userModel)
        {
            bool result = await _iUserService.CreateUser(userModel);
            if (!result)
            {
                ViewBag.Result = $"Error creating account: { _iUserService.LastError} ";
            }
            else
            {
                ViewBag.Result = $"User { userModel.FirstName} {userModel.LastName} has been created";
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string name, string password)
        {
            bool loginOK = await _iUserService.Login(name, password);
            if (!loginOK)
            {
                ViewBag.Result = "Username or password was wrong";
                return RedirectToAction(nameof(Login));
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name,name),
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserModel userModel)
        {
            await _iUserService.CreateUser(userModel);
            ViewBag.Result = $"User { userModel.FirstName} {userModel.LastName} has been created";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UserModel userModel)
        {
            await _iUserService.CreateUser(userModel);
            ViewBag.Result = $"User { userModel.FirstName} {userModel.LastName} has been created";
            return View();
        }
        [Authorize]
        public async Task<IActionResult> ManageUser()
        {
            //Get user
            ClaimsPrincipal user = HttpContext.User; //ClaimsPricipal
            //ClaimsIdentity identity = user.FindFirst(ClaimTypes.Name);
            string name = user.FindFirst(ClaimTypes.Name).Value;
            UserModel model = await _iUserService.ReadUser(name);

            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ManageUser(UserModel userModel, string submit)
        {
            bool showModel = true;
            switch(submit)
            {
                case "Delete":
                    await _iUserService.DeleteUser(userModel);
                    showModel = false;
                    break;
                case "Save":
                    await _iUserService.UpdateUser(userModel);
                    break;
            }
            if (!showModel)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            return View(userModel);
        }

        public IActionResult ErrorNotLoggedIn() => View();
    }
}