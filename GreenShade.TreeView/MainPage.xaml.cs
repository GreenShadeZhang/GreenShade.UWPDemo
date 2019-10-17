using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GreenShade.TreeView.Models;
using Windows.Storage;
using System.Collections.ObjectModel;
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace GreenShade.TreeView
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            ObservableCollection<Son> DataSource1 = new ObservableCollection<Son>();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            base.OnNavigatedTo(e);
            // StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            var sam1 = await storageFolder.TryGetItemAsync("Template.txt");
            if (sam1 != null)
            {
                StorageFile templateFile1 = await storageFolder.GetFileAsync("Template.txt");
                if (templateFile1 != null)
                {
                    string model = await Windows.Storage.FileIO.ReadTextAsync(templateFile1);
                    if (!string.IsNullOrWhiteSpace(model))
                    {
                        ObservableCollection<Son> appsData = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Son>>(model);
                        foreach (var item in appsData)
                        {
                            DataSource1.Add(item);
                        }
                        TreeView1.ItemsSource = DataSource1;

                    }
                }
            }
            else
            {
                var localFolder = ApplicationData.Current.LocalFolder;

                var originalDbFileUri = new Uri("ms-appx:///Assets/Template.txt");

                var originalDbFile = await StorageFile.GetFileFromApplicationUriAsync(originalDbFileUri);
                if (null != originalDbFile)
                {
                  await originalDbFile.CopyAsync(localFolder, "Template.txt", NameCollisionOption.ReplaceExisting);
                    StorageFile templateFile1 = await storageFolder.GetFileAsync("Template.txt");
                    string model = await Windows.Storage.FileIO.ReadTextAsync(templateFile1);
                    if (!string.IsNullOrWhiteSpace(model))
                    {
                        ObservableCollection<Son> appsData = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Son>>(model);
                        foreach (var item in appsData)
                        {
                            DataSource1.Add(item);
                        }
                        TreeView1.ItemsSource = DataSource1;

                    }
                }
            }
        }
    }

    public class ExplorerItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderTemplate { get; set; }
        public DataTemplate FolderTemplate1 { get; set; }
        public DataTemplate FileTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var explorerItem = (Son)item;
            //var explorerItem = explorer.Content as Son;
            if (!string.IsNullOrEmpty(explorerItem.list_icon))
            {
                return FolderTemplate;
            }
            else if (string.IsNullOrEmpty(explorerItem.list_icon) && explorerItem.sons != null && explorerItem.sons.Count > 0)
            {
                return FolderTemplate1;
            }
            else
            {
                return FileTemplate;
            }
            // return FolderTemplate;
            // return explorerItem.sons!=null ? FolderTemplate : FileTemplate;
        }
    }
}
