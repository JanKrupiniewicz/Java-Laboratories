using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Lab_9 
{
    class Program 
    {
        public static void Main()
        {
            List<Car> myCars = new List<Car>(){
                new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            };

            Console.WriteLine("Task 1: Performing LINEQ Queries ...");
            performLINEQQueries(myCars);

            Console.WriteLine("\nTask 2: Serializing data to XML ...");
            serializeToXML(myCars, "cars.xml");

            List<Car> deserializedCars = deserializeFromXML("cars.xml");
            foreach (var car in deserializedCars)
            {
                Console.WriteLine($"Model: {car.Model}, Engine: {car.Motor.Model}, HorsePower: {car.Motor.HorsePower}, Displacement: {car.Motor.Displacement}");
            }

            Console.WriteLine("\nTask 3: Calculating XPath on XML ...");
            calulcateXPathOnXML("cars.xml");

            Console.WriteLine("\nTask 4: Creating XML from LINEQ ...");
            createXmlFromLinq(myCars);

            Console.WriteLine("\nTask 5: Generating XHTML document ...");
            generateXHTML(myCars);

            Console.WriteLine("\nTask 6: Modyfication of XML document ...");
            modifyXmlDocument("cars.xml");

        }
        private static void performLINEQQueries(List<Car> myCars) 
        { 
            var query1 = myCars.Where(car => car.Model == "A6")
                               .Select(car => new
                               {
                                   engineType = car.Motor.Model == "TDI" ? "diesel" : "petrol",
                                   hppl = (double)car.Motor.HorsePower / car.Motor.Displacement
                               });

            var query2 = query1.GroupBy(car => car.engineType)
                               .Select(group => new
                               {
                                   EngineType = group.Key,
                                   AvgHPPL = group.Average(car => car.hppl)
                               });

            foreach (var group in query2)
            {
                Console.WriteLine($"{group.EngineType}: {group.AvgHPPL}");
            }
        }

        private static void serializeToXML(List<Car> myCars, string fileName) 
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, myCars, ns);
            }
        }

        private static List<Car> deserializeFromXML(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
            List<Car> myCars = null;

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                myCars = (List<Car>)serializer.Deserialize(fs);
            }

            return myCars;
        }

        private static void calulcateXPathOnXML(string fileName)
        {
            XElement rootNode = XElement.Load(fileName);

            double avgHP = (double)rootNode.XPathEvaluate("sum(//car/engine[@model!=\"TDI\"]/HorsePower) div count(//car/engine[@model!=\"TDI\"]/HorsePower)");
            Console.WriteLine($"Average HorsePower (excluding TDI): {avgHP}");

            IEnumerable<XElement> models = rootNode.XPathSelectElements("//car[not(Model = preceding::car/Model)]/Model");
            Console.WriteLine("Unique car models:");
            foreach (var model in models)
            {
                Console.WriteLine(model.Value);
            }
        }

        private static void createXmlFromLinq(List<Car> myCars)
        {
            IEnumerable<XElement> nodes = myCars.Select(car =>
            {
                return new XElement("car",
                    new XElement("Model", car.Model),
                    new XElement("Year", car.Year),
                    new XElement("engine",
                        new XAttribute("model", car.Motor.Model),
                        new XElement("Displacement", car.Motor.Displacement),
                        new XElement("HorsePower", car.Motor.HorsePower)
                    )
                );
            });

            XElement rootNode = new XElement("cars", nodes);
            rootNode.Save("CarsFromLinq.xml");
        }

        private static void generateXHTML(List<Car> myCars)
        {
            XElement table = new XElement("table",
                new XElement("tr",
                    new XElement("th", "Model"),
                    new XElement("th", "Year"),
                    new XElement("th", "Engine_Model"),
                    new XElement("th", "Horse_Power")
                ),
                myCars.Select(car =>
                {
                    return new XElement("tr",
                        new XElement("td", car.Model),
                        new XElement("td", car.Year),
                        new XElement("td", car.Motor.Model),
                        new XElement("td", car.Motor.HorsePower)
                    );
                })
            );

            XDocument xhtmlDocument = new XDocument(
                new XDocumentType("html", null, null, null),
                new XElement("html",
                    new XElement("head"),
                    new XElement("body", table)
                )
            );

            xhtmlDocument.Save("CarsTable.html");
        }
        private static void modifyXmlDocument(string fileName)
        {
            XElement xElement = XElement.Load(fileName);

            foreach (XElement carElement in xElement.Elements("car"))
            {
                XElement horsePowerElement = carElement.Element("engine")?.Element("HorsePower");
                if (horsePowerElement != null)
                {
                    horsePowerElement.Name = "hp";
                }

                XElement yearElement = carElement.Element("Year");
                if (yearElement != null)
                {
                    carElement.Element("engine")?.Add(new XAttribute("Year", yearElement.Value));
                    yearElement.Remove();
                }
            }

            xElement.Save(fileName);
        }
    }
}
