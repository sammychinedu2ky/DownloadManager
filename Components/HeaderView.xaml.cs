using DownloadManager.Data;
using DownloadManager.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Forms;

namespace DownloadManager.Components
{
    /// <summary>
    /// Interaction logic for HeaderView.xaml
    /// </summary>
    public partial class HeaderView : System.Windows.Controls.UserControl
    {
        public HeaderView()
        {
            InitializeComponent();
        }

        private async void onClickDownload(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(urlBox.Text))
                {
                    var httpClient = new HttpClient();
                    var saveFile = new SaveFileDialog();
                    var response = await httpClient.GetAsync(urlBox.Text, HttpCompletionOption.ResponseHeadersRead);
                    var contentLength = Convert.ToInt64(response.Content.Headers.GetValues("Content-Length").First());

                    var acceptRanges = !String.IsNullOrEmpty(response.Headers.GetValues("Accept-Ranges").First());
                    if (acceptRanges is false)
                    {
                        System.Windows.MessageBox.Show("Resource doesn't support Range");
                        return;
                    }
                    // saveFile.FileName =  response.Headers.ToString();
                    saveFile.Filter = "All files (*.*)|*.*";
                    var openSave = saveFile.ShowDialog();
                    if (openSave == System.Windows.Forms.DialogResult.OK)
                    {
                        Debug.WriteLine(saveFile.FileName);
                        using (var myStream = saveFile.OpenFile()) ;
                        var item = new Downloads
                        {
                            FileId = Guid.NewGuid().ToString(),
                            FilePath = saveFile.FileName,
                            Url = urlBox.Text,
                            Pause = false,
                            IsComplete = false,
                            OnlineSize = contentLength

                        };
                        using (var db = new DownloadContext())
                        {
                            db.Downloads.Add(item);
                            await db.SaveChangesAsync();
                            (System.Windows.Application.Current.MainWindow.DataContext as ObservableCollection<Downloads>)!.Insert(0, item!);

                        }
                    }
                    urlBox.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }


        }
    }
}
