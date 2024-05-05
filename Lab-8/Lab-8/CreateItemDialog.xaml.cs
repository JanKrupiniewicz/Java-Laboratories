using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp_Lab8_PodejscieNr2
{
    /// <summary>
    /// Interaction logic for CreateItemDialog.xaml
    /// </summary>
    public partial class CreateItemDialog : Window
    {
        public string ItemName { get; private set; }
        public bool IsFile { get; private set; }
        public FileAttributes Attributes { get; private set; }

        public CreateItemDialog()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ItemName = itemNameTextBox.Text;

            if (typeComboBox.SelectedItem == null)
            {
                IsFile = true;
            }
            else
            {
                IsFile = (typeComboBox.SelectedItem as ComboBoxItem).Content.ToString() == "File";
            }

            Attributes = FileAttributes.Normal;
            if (archiveCheckBox.IsChecked == true)
                Attributes |= FileAttributes.Archive;
            if (hiddenCheckBox.IsChecked == true)
                Attributes |= FileAttributes.Hidden;
            if (readOnlyCheckBox.IsChecked == true)
                Attributes |= FileAttributes.ReadOnly;
            if (systemCheckBox.IsChecked == true)
                Attributes |= FileAttributes.System;

            DialogResult = true;
            Close();
        }
    }
}
