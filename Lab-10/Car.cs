using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab_10
{
    public class Car
    {
        public string Model { get; set; }
        public int Year { get; set; }

        public Engine Motor { get; set; }

        public Car()
        {
            Model = "";
            Motor = new Engine();
            Year = 0;
        }

        public Car(string model, Engine motor, int year)
        {
            Model = model;
            Motor = motor;
            this.Year = year;
        }
    }
}
