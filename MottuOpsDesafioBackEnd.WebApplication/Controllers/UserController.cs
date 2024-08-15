using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Text.Json;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
    }
}
