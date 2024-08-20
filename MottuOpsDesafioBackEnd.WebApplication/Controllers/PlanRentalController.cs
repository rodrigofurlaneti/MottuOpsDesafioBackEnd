using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class PlanRentalController : Controller
    {
        private readonly IPlanRentalService _planRentalService;

        public PlanRentalController(IPlanRentalService planRentalService)
        {
            _planRentalService = planRentalService;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var motorcycleModel = await _planRentalService.GetAllAsync();

            return View(motorcycleModel);
        }

        public async Task<ActionResult<int>> Post([FromForm] PlanRentalModel planRentalModel)
        {
            if (planRentalModel == null)
            {
                return BadRequest("A solicitação do plano é nula");
            }

            try
            {
                var planRental = await _planRentalService.PostAsync(planRentalModel);

                if (planRental == 0)
                {
                    TempData["PlanRentalErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                TempData["PlanRentalSuccess"] = "Valido";

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a autenticação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }


        public async Task<ActionResult<PlanRentalModel>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest("A solicitação para atualizar o plano é nula");
            }

            try
            {
                var planRental = await _planRentalService.GetByIdAsync(id);

                if (planRental == null)
                {
                    TempData["PlanRentalErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                return View(planRental);
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a solicitação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }

        public async Task<IActionResult> Update([FromForm] PlanRentalModel planRentalModel)
        {
            if (planRentalModel == null)
            {
                return BadRequest("A solicitação para atualizar o plano é nula");
            }

            try
            {
                await _planRentalService.PutAsync(planRentalModel);

                TempData["PlanRentalUpdateSuccess"] = "Valido";

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro durante a solicitação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            } 
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("A solicitação para apagar o plano é nula");
            }

            await _planRentalService.DeleteAsync(id);

            TempData["PlanRentalDeleteSuccess"] = "Valido";

            return RedirectToAction("Index", "Authentication");
        }
    }
}
