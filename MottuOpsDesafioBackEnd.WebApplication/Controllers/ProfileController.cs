using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Data.Repository;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Text.Json;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult<List<UserProfileModel>>> GetAll()
        {
            var userProfile = await _profileRepository.GetAllAsync();

            return View(userProfile);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] UserProfileModel userProfileModel)
        {
            if (userProfileModel == null)
            {
                return BadRequest("A solicitação do perfil do usuário é nula");
            }

            try
            {
                var userProfile = await _profileRepository.PostAsync(userProfileModel);

                if (userProfile == null)
                {
                    TempData["UserProfileErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                TempData["UserProfileSuccess"] = "Valido";

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a autenticação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileModel>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest("A solicitação para atualizar o perfil do usuário é nula");
            }

            try
            {
                var userProfile = await _profileRepository.GetByIdAsync(id);

                if (userProfile == null)
                {
                    TempData["UserProfileErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                return View(userProfile);
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a autenticação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UserProfileModel userProfileModel)
        {
            if (userProfileModel == null)
            {
                return BadRequest("A solicitação do perfil do usuário é nula");
            }

            try
            {
                await _profileRepository.PutAsync(userProfileModel);

                TempData["UserProfileUpdateSuccess"] = "Valido";

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a autenticação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            } // Se houver erros, retorne a view com o modelo para correção
        }

    }
}
