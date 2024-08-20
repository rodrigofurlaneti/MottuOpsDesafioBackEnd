using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Business.Service;
using System.Reflection;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class ReturnMotorcycleController : Controller
    {
        private readonly IMotorcycleRentalService _motorcycleRentalService;
        private readonly IPlanRentalService _planRentalService;
        public ReturnMotorcycleController(IMotorcycleRentalService motorcycleRentalService, IPlanRentalService planRentalService)
        {
            _motorcycleRentalService = motorcycleRentalService;
            _planRentalService = planRentalService;
        }

        public async Task<IActionResult> Index(int id)
        {
            if (id == 0)
            {
                return BadRequest("A solicitação para atualizar a moto é nula");
            }

            try
            {
                var motorcycleRental = await _motorcycleRentalService.GetByCourierIdAsync(id);

                if (motorcycleRental == null)
                {
                    TempData["MotorcycleRentalErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                motorcycleRental.PlansRental = await _planRentalService.GetAllAsync();

                motorcycleRental.Plans = motorcycleRental.PlansRental.First(x => x.Days == motorcycleRental.PlanType);

                int diasDeLocacao = (motorcycleRental.EndDate - motorcycleRental.StartDate).Days;

                decimal valorTotalSemMulta = diasDeLocacao * Convert.ToDecimal(motorcycleRental.Plans.Value.Replace("R$ ",""));

                ViewBag.ValorTotal = valorTotalSemMulta;

                return View(motorcycleRental);
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a solicitação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }
    }
}
