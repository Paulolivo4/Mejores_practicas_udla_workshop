using BestPractices.ModelBuilders;

namespace BestPractices.Factories
{
    /// Fábrica concreta para crear vehículos tipo Mustang.
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

