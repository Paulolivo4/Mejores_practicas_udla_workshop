using Best_Practices.Models;
using System.Collections.Generic;

namespace BestPractices.Repositories
{
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

