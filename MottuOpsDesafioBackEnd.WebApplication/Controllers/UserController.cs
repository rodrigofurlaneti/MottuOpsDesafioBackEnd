using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Text.Json;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        private readonly IProfileRepository _profileRepository;

        public UserController(IUserRepository userRepository, IProfileRepository profileRepository)
        {
            _userRepository = userRepository;

            _profileRepository = profileRepository;
        }

        public async Task<ActionResult> Index()
        {
            var profiles = await _profileRepository.GetAllAsync();

            var model = new UserModel
            {
                Profiles = profiles.ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> GetAll()
        {
            var userModel = await _userRepository.GetAllAsync();

            return View(userModel);
        }

        public async Task<ActionResult<int>> Post([FromForm] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest("A solicitação do usuário é nula");
            }

            try
            {
                var userProfile = await _userRepository.PostAsync(userModel);

                if (userProfile == 0)
                {
                    TempData["UserErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                TempData["UserSuccess"] = "Valido";

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a autenticação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }


        public async Task<ActionResult<UserModel>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest("A solicitação para atualizar o usuário é nula");
            }

            try
            {
                var user = await _userRepository.GetByIdAsync(id);

                if (user == null)
                {
                    TempData["UserErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                var profiles = await _profileRepository.GetAllAsync();

                user.Profiles = profiles.ToList();

                return View(user);
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a solicitação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }

        public async Task<IActionResult> Update([FromForm] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest("A solicitação do usuário é nula");
            }

            try
            {
                await _userRepository.PutAsync(userModel);

                TempData["UserUpdateSuccess"] = "Valido";

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a solicitação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            } // Se houver erros, retorne a view com o modelo para correção
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("A solicitação para apagar o usuário é nula");
            }

            await _userRepository.DeleteAsync(id);

            TempData["UserDeleteSuccess"] = "Valido";

            return RedirectToAction("Index", "Authentication");
        }



    }
}
