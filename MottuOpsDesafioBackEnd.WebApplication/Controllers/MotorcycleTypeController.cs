using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class MotorcycleTypeController : Controller
    {
        private readonly IMotorcycleTypeService _motorcycleTypeService;

        public MotorcycleTypeController(IMotorcycleTypeService motorcycleTypeService)
        {
            _motorcycleTypeService = motorcycleTypeService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var motorcycleTypeModel = await _motorcycleTypeService.GetAllAsync();

            return View(motorcycleTypeModel);
        }

        public async Task<ActionResult<int>> Post([FromForm] MotorcycleTypeModel motorcycleTypeModel)
        {
            if (motorcycleTypeModel == null)
            {
                return BadRequest("A solicitação do tipo da moto é nula");
            }

            try
            {
                var typeModel = await _motorcycleTypeService.PostAsync(motorcycleTypeModel);

                if (typeModel == 0)
                {
                    TempData["MotorcyclerTypeErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                TempData["MotorcycleTypeSuccess"] = "Valido";

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante o tipo da moto: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }


        public async Task<ActionResult<MotorcycleTypeModel>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest("A solicitação para atualizar a moto é nula");
            }

            try
            {
                var motorcycleType = await _motorcycleTypeService.GetByIdAsync(id);

                if (motorcycleType == null)
                {
                    TempData["MotorcycleTypeErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                return View(motorcycleType);
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a solicitação do tipo da moto: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }

        public async Task<IActionResult> Update([FromForm] MotorcycleTypeModel motorcycleTypeModel)
        {
            if (motorcycleTypeModel == null)
            {
                return BadRequest("A solicitação do tipo da moto é nula");
            }

            try
            {
                await _motorcycleTypeService.PutAsync(motorcycleTypeModel);

                TempData["MotorcycleTypeUpdateSuccess"] = "Valido";

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a solicitação do tipo da moto: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            } // Se houver erros, retorne a view com o modelo para correção
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("A solicitação para apagar o usuário é nula");
            }

            await _motorcycleTypeService.DeleteAsync(id);

            TempData["MotorcycleTypeDeleteSuccess"] = "Valido";

            return RedirectToAction("Index", "Authentication");
        }
    }
}
