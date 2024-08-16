using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Text.Json;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult<AuthenticationResponse>> Dashboard([FromForm] AuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest == null)
            {
                return BadRequest("A solicitação de autenticação é nula");
            }

            try
            {
                var authenticationResponse = await _authenticationService.PostAsync(authenticationRequest);

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
