using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab_9
{
    public class Engine
    {
        public double Displacement { get; set; }
        public int HorsePower { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        public Engine() 
        { 
            Displacement = 0;
            HorsePower = 0;
            Model = "";
        }

        public Engine(double displacement, int hoursePower, string model) { 
            Displacement = displacement;
            HorsePower = hoursePower;
            Model = model;
        }
    }
}
