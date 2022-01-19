using DownloadManager.Data;
using DownloadManager.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace DownloadManager.Components
{
    /// <summary>
    /// Interaction logic for DownloadView.xaml
    /// </summary>
    public partial class DownloadView : System.Windows.Controls.UserControl
    {
        CancellationTokenSource source = new CancellationTokenSource();
        public DownloadView()
        {
            InitializeComponent();
            this.Loaded += DownloadView_Loaded;

        }

        private async void DownloadView_Loaded(object sender, RoutedEventArgs e)
        {
            var context = DataContext as Downloads;
            handleLoadState();
            if (context!.IsComplete == false && context.Pause == false)
            {
                await Task.Run(() => handleDownload());


            }

        }

        private async Task handleDownload()
        {
            try
            {
                Downloads context = new Downloads();
                this.Dispatcher.Invoke(() =>
                {
                    context = (DataContext as Downloads)!;
                });
                var FileSize = () => (new FileInfo(context.FilePath).Length);
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Range = new RangeHeaderValue(FileSize(), null);
                var response = await httpClient.GetStreamAsync(context.Url, source.Token);

               
                using var fStream = new FileStream(context.FilePath, FileMode.Append);
                while (true)
                {
                    if (source.IsCancellationRequested)
                    {
                        break;
                    }
                    var buffer = new byte[context.OnlineSize];
                    var readBytes = await response.ReadAsync(buffer, 0, buffer.Length);

                    if (readBytes == 0)
                    {

                        handleLoadState();
                        break;
                    }

                    fStream.Write(buffer, 0, readBytes);
                    var Progress = () =>
                    {
                        if (context.OnlineSize == 0) return 100;
                        return ((double)new FileInfo(context.FilePath).Length / context.OnlineSize) * 100;
                    };
                    this.Dispatcher.Invoke(() =>
                    {
                        context.IsComplete = true;
                        context.Pause = true;
                        context.Progress = Progress();
                        context.LocalSize = new FileInfo(context.FilePath).Length;
                    });



                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

        }

        private void handleLoadState()
        {

            this.Dispatcher.Invoke(() =>
            {
                var context = DataContext as Downloads;
                if (context != null)
                {
                    if (context.IsComplete)
                    {

                        PlayImg.Visibility = Visibility.Collapsed;
                        PauseImg.Visibility = Visibility.Collapsed;
                        FolderImg.Visibility = Visibility.Visible;

                    }
                    if (!context.IsComplete && !context.Pause)
                    {
                        FolderImg.Visibility = Visibility.Collapsed;
                        PlayImg.Visibility = Visibility.Collapsed;
                        PauseImg.Visibility = Visibility.Visible;
                    }
                    if (!context.IsComplete && context.Pause)
                    {
                        FolderImg.Visibility = Visibility.Collapsed;
                        PauseImg.Visibility = Visibility.Collapsed;
                        PlayImg.Visibility = Visibility.Visible;
                    }
                }
            });



        }

        private void clickPause(object sender, MouseButtonEventArgs e)
        {
            source.Cancel();

            FolderImg.Visibility = Visibility.Collapsed;
            PauseImg.Visibility = Visibility.Collapsed;
            PlayImg.Visibility = Visibility.Visible;
        }

        private void clickPlay(object sender, MouseButtonEventArgs e)
        {
            source = new CancellationTokenSource();
            FolderImg.Visibility = Visibility.Collapsed;
            PlayImg.Visibility = Visibility.Collapsed;
            PauseImg.Visibility = Visibility.Visible;
            Task.Run(() => handleDownload());

        }

        private void clickFolder(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = (DataContext as Downloads).FileName;
            openFileDialog.ShowDialog();
        }

        private void clickDelete(object sender, MouseButtonEventArgs e)
        {
            source.Cancel();
            using (var db = new DownloadContext())
            {
                try
                {
                    db.Remove((Downloads)DataContext);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }



            }
            System.Windows.Application.Current.MainWindow.DataContext = GetData();
        }
        private ObservableCollection<Downloads> GetData()
        {
            using (var db = new DownloadContext())
            {


                var list = db.Downloads.ToList();
                list.Reverse();
                var result = list.Select(i =>
                {
                    var IsComplete = () =>
                    {
                        if (i.OnlineSize == new FileInfo(i.FilePath).Length)
                        {
                            return true;
                        }
                        return false;
                    };
                    var Progress = () =>
                    {
                        if (i.OnlineSize == 0 & new FileInfo(i.FilePath).Length == 0)
                        {
                            return 100;
                        }
                        if (i.OnlineSize == 0) return 100;
                        return ((double)new FileInfo(i.FilePath).Length / i.OnlineSize) * 100;
                    };
                    return new Downloads
                    {
                        IsComplete = IsComplete(),
                        FileId = i.FileId,
                        FilePath = i.FilePath,
                        Url = i.Url,
                        OnlineSize = i.OnlineSize,
                        LocalSize = new FileInfo(i.FilePath).Length,
                        Progress = Progress(),
                        Pause = true


                    };
                }).ToList();

                return new ObservableCollection<Downloads>(result);
            }
        }
    }
}
