using BestPractices.ModelBuilders;

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
