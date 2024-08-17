using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class MotorcycleController : Controller
    {
        private readonly IMotorcycleService _motorcycleService;

        private readonly IMotorcycleTypeService _motorcycleTypeService;

        public MotorcycleController(IMotorcycleService motorcycleService, IMotorcycleTypeService motorcycleTypeService)
        {
            _motorcycleService = motorcycleService;
            _motorcycleTypeService = motorcycleTypeService;
        }


        public async Task<IActionResult> Index()
        {
            var motorcycleType = await _motorcycleTypeService.GetAllAsync();

            var model = new MotorcycleModel
            {
                Models = motorcycleType.ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> GetAll()
        {
            var motorcycleModel = await _motorcycleService.GetAllAsync();

            return View(motorcycleModel);
        }

        public async Task<ActionResult<int>> Post([FromForm] MotorcycleModel motorcycleModel)
        {
            if (motorcycleModel == null)
            {
                return BadRequest("A solicitação do moto é nula");
            }

            try
            {
                var userProfile = await _motorcycleService.PostAsync(motorcycleModel);

                if (userProfile == 0)
                {
                    TempData["MotorcycleErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                TempData["MotorcycleSuccess"] = "Valido";

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a autenticação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }


        public async Task<ActionResult<MotorcycleModel>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest("A solicitação para atualizar a moto é nula");
            }

            try
            {
                var motorcycle = await _motorcycleService.GetByIdAsync(id);

                if (motorcycle == null)
                {
                    TempData["MotorcycleErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                return View(motorcycle);
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a solicitação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }

        public async Task<IActionResult> Update([FromForm] MotorcycleModel motorcycleModel)
        {
            if (motorcycleModel == null)
            {
                return BadRequest("A solicitação do usuário é nula");
            }

            try
            {
                await _motorcycleService.PutAsync(motorcycleModel);

                TempData["MotorcycleUpdateSuccess"] = "Valido";

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

            await _motorcycleService.DeleteAsync(id);

            TempData["MotorcycleDeleteSuccess"] = "Valido";

            return RedirectToAction("Index", "Authentication");
        }
    }
}
