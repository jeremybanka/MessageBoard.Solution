using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

using MessageBoard.Models;
using MessageBoard.ViewModels;

namespace SweetSavory.Controllers
{
  [AllowAnonymous]
  public class AccountController : Controller
  {
    private readonly MessageBoardContext _db;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    public AccountController(
      UserManager<AppUser> userManager,
      SignInManager<AppUser> signInManager,
      MessageBoardContext db
    )
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }

    public async Task<AppUser> Who() =>
      await _userManager.FindByIdAsync(
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
      );

    public async Task<IdentityResult> AddUser(string password, string userName, string firstName, string lastName)
    {
      AppUser au = new(userName, firstName, lastName);
      IdentityResult ir = await _userManager.CreateAsync(au, password);
      return ir;
    }

    [HttpGet] public async Task<ActionResult> Index() => View(await Who());

    [HttpGet] public ActionResult Register() => View();
    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel rvm)
    {
      Console.WriteLine("attempt to register");
      IdentityResult ir = AddUser(rvm.Password, rvm.Email, rvm.FirstName, rvm.LastName);
      if (ir.Succeeded) return RedirectToAction("Index");
      return View();
    }

    [HttpGet] public ActionResult Login() => View();
    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel lvm)
    {
      Microsoft.AspNetCore.Identity.SignInResult sir
      =
      await _signInManager.PasswordSignInAsync(
          lvm.Email,
          lvm.Password,
          isPersistent: true,
          lockoutOnFailure: false
      );
      if (sir.Succeeded) return RedirectToAction("Index");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Logout()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }
  }
}
