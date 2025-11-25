//using EcommerceApp.Models;
//using EcommerceApp.Services;
using EcommerceWebApp.Models;
using EcommerceWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Email and password are required.");
            return View();
        }

        var isValid = await _userService.ValidatePasswordAsync(email, password);

        if (!isValid)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        var user = await _userService.GetUserByEmailAsync(email);

        if (user == null)
        {
            ModelState.AddModelError("", "User not found.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
        };

        if (user.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(User user, string password, string confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || password != confirmPassword)
        {
            ModelState.AddModelError("", "Passwords do not match.");
            return View(user);
        }

        if (password.Length < 6)
        {
            ModelState.AddModelError("", "Password must be at least 6 characters.");
            return View(user);
        }

        var existingUser = await _userService.GetUserByEmailAsync(user.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("", "Email already registered.");
            return View(user);
        }

        if (ModelState.IsValid)
        {
            user.IsAdmin = false;
            await _userService.CreateUserAsync(user, password);
            TempData["Success"] = "Registration successful! Please log in.";
            return RedirectToAction(nameof(Login));
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}