using Best_Practices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestPractices.Repositories;
using BestPractices.Factories;
using BestPractices.Models;
using BestPractices.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BestPractices.Repositories
{
    public interface IVehicleRepository
    {
        ICollection<Vehicle> GetVehicles();
        void AddVehicle(Vehicle vehicle);
        Vehicle Find(string id);
    }

    public class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles = new List<Vehicle>();

        public ICollection<Vehicle> GetVehicles()
        {
            return _vehicles;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
        }

        public Vehicle Find(string id)
        {
            return _vehicles.Find(v => v.Id == id);
        }
    }
}

namespace BestPractices.Factories
{
    public abstract class VehicleFactory
    {
        public abstract Vehicle CreateVehicle();
    }
}

// filepath: BestPractices/Factories/MustangFactory.cs
namespace BestPractices.Factories
{
    public class MustangFactory : VehicleFactory
    {
        public override Vehicle CreateVehicle()
        {
            return new CarBuilder()
                .SetBrand("Ford")
                .SetModel("Mustang")
                .SetColor("Red")
                .Build();
        }
    }
}

// filepath: BestPractices/Factories/ExplorerFactory.cs
namespace BestPractices.Factories
{
    public class ExplorerFactory : VehicleFactory
    {
        public override Vehicle CreateVehicle()
        {
            return new CarBuilder()
                .SetBrand("Ford")
                .SetModel("Explorer")
                .SetColor("Blue")
                .Build();
        }
    }
}

// filepath: BestPractices/Factories/EscapeFactory.cs
namespace BestPractices.Factories
{
    public class EscapeFactory : VehicleFactory
    {
        public override Vehicle CreateVehicle()
        {
            return new CarBuilder()
                .SetBrand("Ford")
                .SetModel("Escape")
                .SetColor("White")
                .Build();
        }
    }
}

public class CarBuilder
{
    private string _brand = "Ford";
    private string _model = "Mustang";
    private string _color = "Red";
    private int _year = DateTime.Now.Year;

    // Puedes agregar más propiedades aquí...

    public CarBuilder SetBrand(string brand)
    {
        _brand = brand;
        return this;
    }

    public CarBuilder SetModel(string model)
    {
        _model = model;
        return this;
    }

    public CarBuilder SetColor(string color)
    {
        _color = color;
        return this;
    }

    public CarBuilder SetYear(int year)
    {
        _year = year;
        return this;
    }

    // Métodos para más propiedades...

    public Car Build()
    {
        return new Car
        {
            Brand = _brand,
            Model = _model,
            Color = _color,
            Year = _year
            // Asigna aquí más propiedades si agregas más
        };
    }
}

namespace BestPractices.Controllers
{
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
            string error = Request.Query.ContainsKey("error") ? Request.Query["error"].ToString() : null;
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
                // lógica para encender el motor...
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
