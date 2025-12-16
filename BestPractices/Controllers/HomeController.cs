using BestPractices.Repositories;
using BestPractices.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace BestPractices.Controllers
{
    /// Controlador principal de la aplicación.
    /// Su responsabilidad es coordinar las acciones entre la vista,
    /// las fábricas y el repositorio.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVehicleRepository _vehicleRepository;

        public HomeController(IVehicleRepository vehicleRepository, ILogger<HomeController> logger)
        {
            _vehicleRepository = vehicleRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();
            model.Vehicles = _vehicleRepository.GetVehicles();

            string error = Request.Query.ContainsKey("error")
                ? Request.Query["error"].ToString()
                : null;

            ViewBag.ErrorMessage = error;
            return View(model);
        }

        [HttpGet]
        public IActionResult AddMustang()
        {
            var factory = new MustangFactory();
            var vehicle = factory.CreateVehicle();
            _vehicleRepository.AddVehicle(vehicle);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddExplorer()
        {
            var factory = new ExplorerFactory();
            var vehicle = factory.CreateVehicle();
            _vehicleRepository.AddVehicle(vehicle);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddEscape()
        {
            var factory = new EscapeFactory();
            var vehicle = factory.CreateVehicle();
            _vehicleRepository.AddVehicle(vehicle);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult StartEngine(string id)
        {
            try
            {
                var vehicle = _vehicleRepository.Find(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al encender el motor");
                return RedirectToAction("Index", new { error = "No se pudo encender el motor" });
            }
        }
    }
}
