
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.Examples
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

        private CancellationTokenSource _operationCancellation;

        private async void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            //var task = Task.Run(() => LongOperation(250, 100));

            //var result = await task;

            var cts = new CancellationTokenSource();
            _operationCancellation = cts;

            var progress = new Progress<double>(p => OperationProgress.Value = p * 100);

            try
            {
                var result = await LongOperation(50, Progress: progress, Cancel: cts.Token);
                ResultTextBlock.Text = result;
            }
            catch(OperationCanceledException ex)
            {
                ResultTextBlock.Text = "Операция отменена."; 
            }

            ((IProgress<double>)progress).Report(0);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            _operationCancellation?.Cancel();
        }

        private async Task<string> LongOperation(
            int Timeout, int Count=100, 
            IProgress<double>? Progress = default, 
            CancellationToken Cancel = default)
        {
            Cancel.ThrowIfCancellationRequested();

            Debug.WriteLine("Задача запущена в потоке {0}", Thread.CurrentThread.ManagedThreadId);

            for (int i = 0; i < Count; i++)
            {
                if (Cancel.IsCancellationRequested)
                {
                    Cancel.ThrowIfCancellationRequested();
                    //throw new OperationCanceledException(Cancel);
                }

                //Thread.Sleep(Timeout);
                await Task.Delay(Timeout, Cancel).ConfigureAwait(false);    //true/false -  выполнять/не выполнять задачу в 1 потоке
                Debug.WriteLine("Итерация обработки данных {0} в потоке {1}", i, Thread.CurrentThread.ManagedThreadId);

                Progress?.Report((double)i / Count);
            }

            Cancel.ThrowIfCancellationRequested();
            return DateTime.Now.ToLongTimeString();
        }
    }
}
