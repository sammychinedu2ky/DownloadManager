global using System.Diagnostics;
using DownloadManager.Data;
using DownloadManager.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = GetData();
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
