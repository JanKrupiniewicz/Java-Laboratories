using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab_10
{
    public class Engine : IComparable
    {
        public double Displacement { get; set; }
        public int HorsePower { get; set; }

        public string Model { get; set; }

        public Engine()
        {
            Displacement = 0;
            HorsePower = 0;
            Model = "";
        }

        public Engine(double displacement, int hoursePower, string model)
        {
            Displacement = displacement;
            HorsePower = hoursePower;
            Model = model;
        }

        public int CompareTo(Object obj)
        {
            Engine other = obj as Engine;
            if (other == null) return 1;
            return this.HorsePower.CompareTo(other.HorsePower);
        }
    }
}
