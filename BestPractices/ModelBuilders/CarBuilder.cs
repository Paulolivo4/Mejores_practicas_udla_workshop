using Best_Practices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestPractices.ModelBuilders
{
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
            };
        }
    }
}

