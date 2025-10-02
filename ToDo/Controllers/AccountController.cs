using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoTest.Models;
using ToDoTest.Services;
using ToDoTest.Views.ViewModels;

namespace ToDoTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly IToDoService _toDoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IToDoService toDoService, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _toDoService = toDoService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded) return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Neispravno korisničko ime ili lozinka");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User); 
            var todos = await _toDoService.GetUserToDosAsync(userId);

            return View(todos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ToDo todo)
        {
            var userId = _userManager.GetUserId(User);
            todo.UserId = userId;

            await _toDoService.AddToDoAsync(todo);

            return RedirectToAction(nameof(Index));
        }
    }
}
