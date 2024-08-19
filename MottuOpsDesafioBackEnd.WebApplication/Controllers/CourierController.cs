using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.WebApplication.Controllers
{
    public class CourierController : Controller
    {
        private readonly ICourierService _courierService;

        public CourierController(ICourierService courierService)
        {
            _courierService = courierService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Post([FromForm] CourierModel courierModel)
        {
            if (courierModel == null)
            {
                return BadRequest("A solicitação do entregador é nula");
            }

            try
            {
                await UploadArchiveCnh(courierModel);

                var courier = await _courierService.PostAsync(courierModel);

                if (courier == 0)
                {
                    TempData["CourierErro"] = "Ocorreu um erro ao tentar salvar o entregador.";
                    return RedirectToAction("Index", "Home");
                }

                TempData["CourierSuccess"] = "Valido";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro durante a criação do entregador: {ex.Message}");
                return StatusCode(500, "Erro do Servidor Interno");
            }
        }

        public async Task<ActionResult<CourierModel>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest("A solicitação para atualizar o entregador é nula");
            }

            try
            {
                var courier = await _courierService.GetByIdAsync(id);

                if (courier == null)
                {
                    TempData["CourierErro"] = "Invalido";

                    return RedirectToAction("Index", "Authentication");
                }

                return View(courier);
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a solicitação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            }
        }

        public async Task<IActionResult> UpdateCnh([FromForm] CourierModel courierModel)
        {
            if (courierModel == null)
            {
                return BadRequest("A solicitação do usuário é nula");
            }

            try
            {
                await UploadArchiveCnh(courierModel);

                await _courierService.PutCnhAsync(courierModel);

                TempData["CourierUpdateCnhSuccess"] = "Valido";

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.Error.WriteLine($"Erro durante a solicitação: {ex.Message}");

                return StatusCode(500, "Erro do Servidor Interno");
            } // Se houver erros, retorne a view com o modelo para correção
        }

        public async Task<JsonResult> CnpjExist(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
            {
                return Json(false);
            }

            try
            {
                var existCnpj = await _courierService.GetByCnpjAsync(cnpj);
                return Json(existCnpj);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro durante a verificação do CNPJ: {ex.Message}");
                return Json(false); 
            }
        }

        public async Task<JsonResult> CnhExist(string cnh)
        {
            if (string.IsNullOrEmpty(cnh))
            {
                return Json(false);
            }

            try
            {
                var existCnh = await _courierService.GetByCnhAsync(cnh);
                return Json(existCnh);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro durante a verificação da CNH: {ex.Message}");
                return Json(false); 
            }
        }

        private async Task<CourierModel> UploadArchiveCnh(CourierModel courierModel)
        {
            if (courierModel.CNHImagePathFormFile != null && courierModel.CNHImagePathFormFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "CNHImages");

                var formattedDate = string.Format("{0:s}", DateTime.Now);

                var dateClean = formattedDate.Replace("-", "_").Replace(":", "_");

                var uniqueFileName = dateClean + "_" + courierModel.CNHImagePathFormFile.FileName;

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await courierModel.CNHImagePathFormFile.CopyToAsync(fileStream);
                }

                courierModel.CNHImagePath = uniqueFileName;
            }

            return courierModel;
        }
    }
}
