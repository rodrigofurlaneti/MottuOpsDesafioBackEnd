using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Text.Json;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationController(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> Dashboard([FromForm] AuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest == null)
            {
                return BadRequest("A solicitação de autenticação é nula");
            }

            try
            {
                var authenticationResponse = await _authenticationRepository.PostAsync(authenticationRequest);

                if (authenticationResponse == null)
                {
                    TempData["AuthenticationErro"] = "Invalido";
                    return RedirectToAction("Index", "Home");
                }

                HttpContext.Session.SetString("AuthResponse", JsonSerializer.Serialize(authenticationResponse));

                TempData["AuthenticationSuccess"] = "Valido";

                return View();
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a autenticação: {ex.Message}");
                return StatusCode(500, "Erro do Servidor Interno");
            }
        }
    }
}
