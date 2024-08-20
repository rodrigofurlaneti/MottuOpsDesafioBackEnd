using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Business.Service;
using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class MotorcycleRentalController : Controller
    {
        private readonly IMotorcycleRentalService _motorcycleRentalService;

        private readonly IMotorcycleService _motorcycleService;

        private readonly IPlanRentalService _planRentalService;

        private readonly ICourierService _courierService;

        public MotorcycleRentalController(IMotorcycleRentalService motorcycleRentalService, 
            IMotorcycleService motorcycleService, IPlanRentalService planRentalService, ICourierService courierService)
        {
            _motorcycleRentalService = motorcycleRentalService;
            _motorcycleService = motorcycleService;
            _planRentalService = planRentalService;
            _courierService = courierService;
        }

        public async Task<IActionResult> Index()
        {
            var motorcycles = await _motorcycleService.GetAllAsync();

            var plans = await _planRentalService.GetAllAsync();

            var model = new MotorcycleRentalModel
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.UtcNow,
                ExpectedEndDate = DateTime.UtcNow,
                Motorcycles = motorcycles.ToList(),
                PlansRental = plans.ToList()
            };

            return View(model);
        }

        public async Task<ActionResult<int>> Post([FromForm] MotorcycleRentalModel motorcycleRentalModel)
        {
            if (motorcycleRentalModel == null)
            {
                return BadRequest("A solicitação do moto é nula");
            }

            try
            {
                var courier = await _courierService.GetByIdAsync(motorcycleRentalModel.CourierId);

                if (courier.CNHType.Equals("A") || courier.CNHType.Equals("A+B"))
                {
                    var motorcycleRental = await _motorcycleRentalService.PostAsync(motorcycleRentalModel);

                    if (motorcycleRental == 0)
                    {
                        TempData["MotorcycleRentalErro"] = "Invalido";

                        return RedirectToAction("Index", "Authentication");
                    }

                    TempData["MotorcycleRentalSuccess"] = "Valido";

                    return RedirectToAction("Index", "Authentication");
                }
                else
                {
                    TempData["MotorcycleRentalCNHTypeBErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }
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
