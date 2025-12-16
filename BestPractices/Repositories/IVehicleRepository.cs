using Best_Practices.Models;
using System.Collections.Generic;

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
}
