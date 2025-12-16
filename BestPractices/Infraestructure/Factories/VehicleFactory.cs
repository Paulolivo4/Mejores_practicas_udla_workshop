using BestPractices.Models;

namespace BestPractices.Factories
{
    /// Fábrica abstracta que define el método para crear vehículos
    /// Implementa el patrón Factory Method
    public abstract class VehicleFactory
    {
        public abstract Vehicle CreateVehicle();
    }
}

