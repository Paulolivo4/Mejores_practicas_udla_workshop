using BestPractices.ModelBuilders;

namespace BestPractices.Factories
{
    /// Fábrica concreta para crear vehículos tipo Explorer.
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
