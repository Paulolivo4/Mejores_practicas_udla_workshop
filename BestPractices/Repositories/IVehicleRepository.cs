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
    /// Interfaz del repositorio de vehículos.
    /// Define las operaciones básicas sin depender de una base de datos.
    /// Aplica el principio de Inversión de Dependencias (DIP).
    public interface IVehicleRepository
    {
        // Retorna todos los vehículos almacenados
        ICollection<Vehicle> GetVehicles();

        // Agrega un nuevo vehículo al repositorio
        void AddVehicle(Vehicle vehicle);

        // Busca un vehículo por su identificador
        Vehicle Find(string id);
    }

    /// Implementación del repositorio en memoria.
    /// Permite probar la aplicación sin una base de datos real.
    public class InMemoryVehicleRepository : IVehicleRepository
    {
        // Lista en memoria que simula el almacenamiento de datos
        private readonly List<Vehicle> _vehicles = new List<Vehicle>();

        // Retorna todos los vehículos almacenados en memoria
        public ICollection<Vehicle> GetVehicles()
        {
            return _vehicles;
        }

        // Agrega un vehículo a la lista en memoria
        public void AddVehicle(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
        }

        // Busca un vehículo por su Id
        public Vehicle Find(string id)
        {
            return _vehicles.Find(v => v.Id == id);
        }
    }
}

namespace BestPractices.Factories
{
    /// Fábrica abstracta que define el método para crear vehículos
    /// Implementa el patrón Factory Method
    public abstract class VehicleFactory
    {
        // Método que será implementado por las fábricas concretas
        public abstract Vehicle CreateVehicle();
    }
}

namespace BestPractices.Factories
{
    
    /// Fábrica concreta para crear vehículos tipo Mustang.
    public class MustangFactory : VehicleFactory
    {
        public override Vehicle CreateVehicle()
        {
            // Usa el Builder para construir el vehículo
            return new CarBuilder()
                .SetBrand("Ford")
                .SetModel("Mustang")
                .SetColor("Red")
                .Build();
        }
    }
}

namespace BestPractices.Factories
{
    /// Fábrica concreta para crear vehículos tipo Explorer.
    public class ExplorerFactory : VehicleFactory
    {
        public override Vehicle CreateVehicle()
        {
            // La lógica de creación queda encapsulada en la fábrica
            return new CarBuilder()
                .SetBrand("Ford")
                .SetModel("Explorer")
                .SetColor("Blue")
                .Build();
        }
    }
}

namespace BestPractices.Factories
{
    /// Fábrica concreta para crear vehículos tipo Escape
    /// Permite agregar nuevos modelos sin modificar el controlador
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

/// Builder para la construcción de vehículos
/// Permite asignar propiedades por defecto y facilita la extensión
/// del modelo con nuevas propiedades en futuros sprints
public class CarBuilder
{
    // Propiedades por defecto
    private string _brand = "Ford";
    private string _model = "Mustang";
    private string _color = "Red";
    private int _year = DateTime.Now.Year;

    // Aquí se pueden agregar más propiedades en el futuro

    // Asigna la marca del vehículo
    public CarBuilder SetBrand(string brand)
    {
        _brand = brand;
        return this;
    }

    // Asigna el modelo del vehículo
    public CarBuilder SetModel(string model)
    {
        _model = model;
        return this;
    }

    // Asigna el color del vehículo
    public CarBuilder SetColor(string color)
    {
        _color = color;
        return this;
    }

    // Asigna el año del vehículo
    public CarBuilder SetYear(int year)
    {
        _year = year;
        return this;
    }

    // Construye y retorna el objeto final
    public Car Build()
    {
        return new Car
        {
            Brand = _brand,
            Model = _model,
            Color = _color,
            Year = _year
            // Aquí se asignarían nuevas propiedades si se agregan
        };
    }
}

namespace BestPractices.Controllers
{
    /// Controlador principal de la aplicación.
    /// Su responsabilidad es coordinar las acciones entre la vista,
    /// las fábricas y el repositorio.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVehicleRepository _vehicleRepository;

        /// Se inyecta el repositorio mediante la interfaz,
        /// cumpliendo el principio de Inversión de Dependencias (DIP).
        public HomeController(IVehicleRepository vehicleRepository, ILogger<HomeController> logger)
        {
            _vehicleRepository = vehicleRepository;
            _logger = logger;
        }

        // Muestra la lista de vehículos en la vista principal
        public IActionResult Index()
        {
            var model = new HomeViewModel();

            // Obtiene los vehículos desde el repositorio
            model.Vehicles = _vehicleRepository.GetVehicles();

            // Manejo básico de mensajes de error
            string error = Request.Query.ContainsKey("error") ? Request.Query["error"].ToString() : null;
            ViewBag.ErrorMessage = error;

            return View(model);
        }

        // Agrega un vehículo Mustang usando Factory Method
        [HttpGet]
        public IActionResult AddMustang()
        {
            var factory = new MustangFactory();
            var vehicle = factory.CreateVehicle();
            _vehicleRepository.AddVehicle(vehicle);
            return RedirectToAction("Index");
        }

        // Agrega un vehículo Explorer usando Factory Method
        [HttpGet]
        public IActionResult AddExplorer()
        {
            var factory = new ExplorerFactory();
            var vehicle = factory.CreateVehicle();
            _vehicleRepository.AddVehicle(vehicle);
            return RedirectToAction("Index");
        }

        // Agrega un vehículo Escape usando Factory Method
        [HttpGet]
        public IActionResult AddEscape()
        {
            var factory = new EscapeFactory();
            var vehicle = factory.CreateVehicle();
            _vehicleRepository.AddVehicle(vehicle);
            return RedirectToAction("Index");
        }

        // Ejemplo de acción que usa el repositorio
        [HttpGet]
        public IActionResult StartEngine(string id)
        {
            try
            {
                var vehicle = _vehicleRepository.Find(id);
                // Aquí iría la lógica para encender el motor
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
