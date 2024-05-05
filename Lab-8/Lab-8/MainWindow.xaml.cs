using System.IO;
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

namespace WpfApp_Lab8_PodejscieNr2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PopulateTreeView(string directory, TreeViewItem parentNode)
        {
            string[] subDirectories = Directory.GetDirectories(directory);

            foreach (string subDirectory in subDirectories)
            {
                TreeViewItem directoryNode = new TreeViewItem();
                directoryNode.Header = System.IO.Path.GetFileName(subDirectory);
                directoryNode.Tag = subDirectory;
                parentNode.Items.Add(directoryNode);
                PopulateTreeView(subDirectory, directoryNode);

                ContextMenu directoryMenu = new ContextMenu();
                MenuItem deleteDirectoryMenuItem = new MenuItem();
                deleteDirectoryMenuItem.Header = "Delete";
                deleteDirectoryMenuItem.Click += (sender, e) => DeleteItem(subDirectory);
                directoryMenu.Items.Add(deleteDirectoryMenuItem);
                directoryNode.ContextMenu = directoryMenu;

                MenuItem createFileMenuItem = new MenuItem();
                createFileMenuItem.Header = "Create file";
                createFileMenuItem.Click += (sender, e) => CreateFile(subDirectory);
                directoryMenu.Items.Add(createFileMenuItem);
                directoryNode.ContextMenu = directoryMenu;

            }

            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                TreeViewItem fileNode = new TreeViewItem();
                fileNode.Header = System.IO.Path.GetFileName(file);
                fileNode.Tag = file;
                parentNode.Items.Add(fileNode);

                ContextMenu fileMenu = new ContextMenu();
                MenuItem deleteFileMenuItem = new MenuItem();
                deleteFileMenuItem.Header = "Delete";
                deleteFileMenuItem.Click += (sender, e) => DeleteItem(file);
                fileMenu.Items.Add(deleteFileMenuItem);
                fileNode.ContextMenu = fileMenu;
            }
        }

        private void fileTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (fileTreeView.SelectedValue != null) { 
                TreeViewItem selectedItem = (TreeViewItem)fileTreeView.SelectedValue;
                string path = selectedItem.Tag.ToString();

                if (File.Exists(path))
                {
                    string content = File.ReadAllText(path);
                    fileContentTextBlock.Text = content;
                }
                DisplayFileAttributes(path);
            }
        }

        private void DisplayFileAttributes(string path) { 
            FileAttributes attributes = File.GetAttributes(path);
            string attributesString = string.Empty;

            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                attributesString += "r";
            }
            else
            {
                attributesString += "-";
            }

            if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
            {
                attributesString += "a";
            }
            else
            {
                attributesString += "-";
            }

            if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                attributesString += "h";
            }
            else
            {
                attributesString += "-";
            }   

            if ((attributes & FileAttributes.System) == FileAttributes.System)
            {
                attributesString += "s";
            }
            else
            {
                attributesString += "-";
            }

            statusText.Text = attributesString;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog() { Description = "Select directory to open" };
            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFolder = folderDialog.SelectedPath;
                TreeViewItem rootItem = new TreeViewItem();
                rootItem.Header = "Root";
                rootItem.Tag = selectedFolder;
                fileTreeView.Items.Add(rootItem);
                PopulateTreeView(selectedFolder, rootItem);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateFile(string directory) 
        { 
            CreateItemDialog dialog = new CreateItemDialog();
            if (dialog.ShowDialog() == true) { 
                string fileName = dialog.ItemName;
                FileAttributes attributes = dialog.Attributes;
                bool isFile = dialog.IsFile;

                if (!string.IsNullOrEmpty(fileName))
                {
                    if (isFile)
                    {
                        if (ValidateFileName(fileName))
                        {
                            string filePath = System.IO.Path.Combine(directory, fileName);
                            if (!File.Exists(filePath))
                            {
                                using (File.Create(filePath)) { }

                                File.SetAttributes(filePath, attributes);

                                TreeViewItem fileNode = new TreeViewItem();
                                fileNode.Header = fileName;
                                fileNode.Tag = filePath;

                                ContextMenu fileMenu = new ContextMenu();
                                MenuItem deleteFileMenuItem = new MenuItem();
                                deleteFileMenuItem.Header = "Delete";
                                deleteFileMenuItem.Click += (sender, e) => DeleteItem(filePath);
                                fileMenu.Items.Add(deleteFileMenuItem);
                                fileNode.ContextMenu = fileMenu;

                                TreeViewItem parentNode = FindTreeViewItem(fileTreeView, directory);
                                parentNode.Items.Add(fileNode);
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("File with this name already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("File name contains invalid characters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        string directoryName = System.IO.Path.Combine(directory, fileName);
                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);

                            TreeViewItem folderNode = new TreeViewItem();
                            folderNode.Header = fileName;
                            folderNode.Tag = directoryName;

                            ContextMenu folderMenu = new ContextMenu();
                            MenuItem deleteFolderMenuItem = new MenuItem();
                            deleteFolderMenuItem.Header = "Delete";
                            deleteFolderMenuItem.Click += (sender, e) => DeleteItem(directoryName);
                            folderMenu.Items.Add(deleteFolderMenuItem);

                            MenuItem createFileMenuItem = new MenuItem();
                            createFileMenuItem.Header = "Create File";
                            createFileMenuItem.Click += (sender, e) => CreateFile(directoryName);
                            folderMenu.Items.Add(createFileMenuItem);

                            folderNode.ContextMenu = folderMenu;
                            TreeViewItem parentNode = FindTreeViewItem(fileTreeView, directory);
                            parentNode.Items.Add(folderNode);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Folder with this name already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else 
                {
                    System.Windows.MessageBox.Show("Invalid Input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteItem(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                else if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                // Usuń element z drzewa
                TreeViewItem itemToRemove = FindTreeViewItem(fileTreeView, path);
                if (itemToRemove != null)
                {
                    ((TreeViewItem)itemToRemove.Parent).Items.Remove(itemToRemove);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred while deleting the item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateFileName(string fileName)
        {
            string invalidChars = new string(System.IO.Path.GetInvalidFileNameChars());
            if (fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            {
                return false;
            }
            return true;
        }

        private TreeViewItem FindTreeViewItem(ItemsControl parent, string path)
        {
            foreach (object item in parent.Items)
            {
                TreeViewItem treeItem = (TreeViewItem)item;
                if (treeItem.Tag.ToString() == path)
                {
                    return treeItem;
                }

                TreeViewItem result = FindTreeViewItem(treeItem, path);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}