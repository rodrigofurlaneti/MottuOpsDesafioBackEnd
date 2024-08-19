using Microsoft.AspNetCore.Mvc;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    }
}
