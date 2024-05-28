using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private SortableBindingList<Car> myCarsBindingList;

        private List<Car> myCars = new() {
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

        public MainWindow()
        {
            InitializeComponent();
            BindDataToGrid();
            SetupSearchInterface();

            LINQQuery();
            MethodBasedLINQQuery();
            Delegates();
        }

        private void LINQQuery() 
        {
            var elements = from car in myCarsBindingList
                           where car.Model == "A6"
                           group car by car.Motor.Model == "TDI" ? "diesel" : "petrol" into g
                           let avgHPPL = g.Average(car => car.Motor.HorsePower / car.Motor.Displacement)
                           orderby avgHPPL descending
                           select new { engineType = g.Key, avgHPPL };

            foreach (var e in elements)
            {
                Console.WriteLine(e.engineType + ": " + e.avgHPPL);
                MessageBox.Show(e.engineType + ": " + e.avgHPPL);
            }
        }

        private void MethodBasedLINQQuery()
        {
            var elements = myCarsBindingList
            .Where(car => car.Model == "A6")
            .GroupBy(car => car.Motor.Model == "TDI" ? "diesel" : "petrol")
            .Select(g => new { engineType = g.Key, avgHPPL = g.Average(car => car.Motor.HorsePower / car.Motor.Displacement) })
            .OrderByDescending(e => e.avgHPPL);

            foreach (var e in elements)
            {
                Console.WriteLine(e.engineType + ": " + e.avgHPPL);
                MessageBox.Show(e.engineType + ": " + e.avgHPPL);
            }
        }

        private void Delegates()
        {
            Func<Car, Car, int> arg1 = Func;
            Predicate<Car> arg2 = Predicate;
            Action<Car> arg3 = Action;

            myCars.Sort(new Comparison<Car>(arg1));
            myCars.FindAll(arg2).ForEach(arg3);
        }

        private int Func(Car x, Car y) 
        {
            if (x.Motor.HorsePower > y.Motor.HorsePower)
            {
                return 1;
            }
            else if (x.Motor.HorsePower < y.Motor.HorsePower)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private void OnDeleteSelectedCar(object sender, RoutedEventArgs e)
        {
            var selectedCars = dataGrid.SelectedItems.Cast<Car>().ToList();
            foreach (var car in selectedCars)
            {
                myCarsBindingList.Remove(car);
            }
        }

        private bool Predicate(Car x)
        {
            return x.Motor.Model == "TDI";
        }

        private void Action(Car x)
        { 
            Console.WriteLine(x.Model);
            MessageBox.Show(x.Model);
        }

        private void BindDataToGrid()
        {
            myCarsBindingList = new SortableBindingList<Car>(myCars);
            dataGrid.ItemsSource = myCarsBindingList;
        }

        private void SetupSearchInterface()
        {
            propertyComboBox.Items.Add("Model");
            propertyComboBox.Items.Add("Year");
            propertyComboBox.Items.Add("Motor.HorsePower");
            propertyComboBox.Items.Add("Motor.Model");
            propertyComboBox.Items.Add("Motor.Displacement");
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            string property = propertyComboBox.SelectedItem?.ToString();
            string value = searchTextBox.Text;

            if (property == null || string.IsNullOrEmpty(value))
            {
                MessageBox.Show("Please select a property and enter a value to search for.");
                return;
            }

            var results = myCarsBindingList.Where(car =>
            {
                var propValue = GetPropertyValue(car, property);
                return propValue != null && propValue.ToString().Contains(value);
            }).ToList();

            dataGrid.ItemsSource = new SortableBindingList<Car>(results);
        }

        private void OnSearchFirstClick(object sender, RoutedEventArgs e)
        {
            string property = propertyComboBox.SelectedItem?.ToString();
            string value = searchTextBox.Text;

            if (property == null || string.IsNullOrEmpty(value))
            {
                MessageBox.Show("Please select a property and enter a value to search for.");
                return;
            }

            var result = myCarsBindingList.FirstOrDefault(car =>
            {
                var propValue = GetPropertyValue(car, property);
                return propValue != null && propValue.ToString().Contains(value);
            });

            if (result != null)
            {
                dataGrid.SelectedItem = result;
                dataGrid.ScrollIntoView(result);
            }
        }

        private void OnResetClick(object sender, RoutedEventArgs e)
        {
            myCarsBindingList = new SortableBindingList<Car>(myCars);
            dataGrid.ItemsSource = myCarsBindingList;
        }

        private void SortOnModelClick(object sender, RoutedEventArgs e)
        {
            myCarsBindingList.SortByModel(ListSortDirection.Ascending);
            dataGrid.ItemsSource = myCarsBindingList;
        }

        private void SortOnYearClick(object sender, RoutedEventArgs e)
        {
            myCarsBindingList.SortByYear(ListSortDirection.Ascending);
            dataGrid.ItemsSource = myCarsBindingList;
        }

        private void SortOnMotorClick(object sender, RoutedEventArgs e)
        {
            myCarsBindingList = new SortableBindingList<Car>(myCarsBindingList.OrderBy(car => car.Motor.HorsePower).ToList());
            dataGrid.ItemsSource = myCarsBindingList;
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            if (propertyName.Contains("."))
            {
                var properties = propertyName.Split('.');
                foreach (var prop in properties)
                {
                    if (obj == null) return null;
                    var propInfo = obj.GetType().GetProperty(prop);
                    if (propInfo == null) return null;
                    obj = propInfo.GetValue(obj, null);
                }
                return obj;
            }
            else
            {
                var propInfo = obj.GetType().GetProperty(propertyName);
                return propInfo?.GetValue(obj, null);
            }
        }
    }
}
