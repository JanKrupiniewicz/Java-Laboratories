using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_Lab_11
{
    public partial class MainWindow : Window
    {
        private int? K => int.TryParse(KTextBox.Text, out var k) ? k : (int?)null;
        private int? N => int.TryParse(NTextBox.Text, out var n) ? n : (int?)null;
        private int I => int.TryParse(ITextBox.Text, out var i) ? i : 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateUsingTasksOnClick(object sender, RoutedEventArgs e)
        {
            if (K is null || N is null)
            {
                MessageBox.Show("Invalid Input.");
                return;
            }

            int n = N.Value;
            int k = K.Value;

            Task<int> numeratorTask = Task.Run(() => CalculateNumerator(n, k));
            Task<int> denominatorTask = Task.Run(() => CalculateDenominator(k));

            Task.WhenAll(numeratorTask, denominatorTask).ContinueWith(t =>
            {
                int result = numeratorTask.Result / denominatorTask.Result;
                Dispatcher.Invoke(() => ResultTasks.Text = result.ToString());
            });
        }

        private void CalculateUsingDelegatesOnClick(object sender, RoutedEventArgs e)
        {
            if (K is null || N is null)
            {
                MessageBox.Show("Invalid Input.");
                return;
            }

            int n = N.Value;
            int k = K.Value;

            Func<int, int, int> numeratorFunc = (nn, kk) => CalculateNumerator(nn, kk);
            Func<int, int> denominatorFunc = (kk) => CalculateDenominator(kk);

            IAsyncResult numeratorResult = numeratorFunc.BeginInvoke(n, k, null, null);
            IAsyncResult denominatorResult = denominatorFunc.BeginInvoke(k, null, null);

            int numerator = numeratorFunc.EndInvoke(numeratorResult);
            int denominator = denominatorFunc.EndInvoke(denominatorResult);

            int result = numerator / denominator;
            ResultDelegates.Text = result.ToString();
        }

        private async void CalculateUsingAsyncAwaitOnClick(object sender, RoutedEventArgs e)
        {
            if (K is null || N is null)
            {
                MessageBox.Show("Invalid Input.");
                return;
            }

            int n = N.Value;
            int k = K.Value;

            int numerator = await Task.Run(() => CalculateNumerator(n, k));
            int denominator = await Task.Run(() => CalculateDenominator(k));

            int result = numerator / denominator;
            ResultAsyncAwait.Text = result.ToString();
        }

        private void CalculateIthValueOnClick(object sender, RoutedEventArgs e)
        {
            if (I == 0)
            {
                MessageBox.Show("Invalid Input.");
                return;
            }

            int i = I;
            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            worker.DoWork += (s, args) =>
            {
                int[] fib = new int[i + 1];
                fib[0] = 0;
                if (i > 0)
                    fib[1] = 1;

                for (int j = 2; j <= i; j++)
                {
                    Thread.Sleep(5); // Spowolnienie
                    fib[j] = fib[j - 1] + fib[j - 2];
                    worker.ReportProgress(j * 100 / i, fib[j]);
                }

                args.Result = fib[i];
            };

            worker.ProgressChanged += (s, args) =>
            {
                ResultCalculateIth.Text = args.UserState.ToString();
            };

            worker.RunWorkerCompleted += (s, args) =>
            {
                MessageBox.Show($"Final result: {args.Result}");
            };

            worker.RunWorkerAsync();
        }

        private void CompressOnClick(object sender, RoutedEventArgs e)
        {
            var folderDialog = new Microsoft.Win32.OpenFileDialog();
            folderDialog.CheckFileExists = false;
            folderDialog.CheckPathExists = true;
            folderDialog.ValidateNames = false;
            folderDialog.FileName = "Folder Selection";

            if (folderDialog.ShowDialog() == true)
            {
                string folderPath = Path.GetDirectoryName(folderDialog.FileName);
                string[] files = Directory.GetFiles(folderPath);

                Parallel.ForEach(files, file =>
                {
                    string compressedFile = file + ".gz";
                    using (FileStream originalFileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    using (FileStream compressedFileStream = new FileStream(compressedFile, FileMode.Create))
                    using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                });

                MessageBox.Show("Compression completed.");
            }
        }
        private void DecompressOnClick(object sender, RoutedEventArgs e)
        {
            var folderDialog = new Microsoft.Win32.OpenFileDialog();
            folderDialog.CheckFileExists = false;
            folderDialog.CheckPathExists = true;
            folderDialog.ValidateNames = false;
            folderDialog.FileName = "Folder Selection";

            if (folderDialog.ShowDialog() == true)
            {
                string folderPath = Path.GetDirectoryName(folderDialog.FileName);
                string[] files = Directory.GetFiles(folderPath, "*.gz");

                Parallel.ForEach(files, file =>
                {
                    string decompressedFile = file.Substring(0, file.Length - 3);
                    using (FileStream compressedFileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    using (FileStream decompressedFileStream = new FileStream(decompressedFile, FileMode.Create))
                    using (GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                });

                MessageBox.Show("Decompression completed.");
            }
        }
        private int CalculateNumerator(int n, int k)
        {
            int result = 1;
            for (int i = 0; i < k; i++)
            {
                result *= (n - i);
            }
            return result;
        }

        private int CalculateDenominator(int k)
        {
            int result = 1;
            for (int i = 1; i <= k; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}
