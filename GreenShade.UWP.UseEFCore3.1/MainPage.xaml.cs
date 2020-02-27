using EFcore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace GreenShade.UWP.UseEFCore3._1
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
            var dbFile = await ApplicationData.Current.LocalFolder.TryGetItemAsync("blogging.db") as StorageFile;
            if (null == dbFile || SystemInformation.IsFirstRun)
            {
                // first time ... copy the .db file from assets to local  folder
                var localFolder = ApplicationData.Current.LocalFolder;
                var originalDbFileUri = new Uri("ms-appx:///Assets/blogging.db");
                var originalDbFile = await StorageFile.GetFileFromApplicationUriAsync(originalDbFileUri);
                if (null != originalDbFile)
                {

                    dbFile = await originalDbFile.CopyAsync(localFolder, "blogging.db", NameCollisionOption.ReplaceExisting);

                }
            }
            try
            {
                using (var db = new BloggingContext())
                {
                    //这里是传绝对路径给的数据上下文
                   db.DbFilePath = dbFile.Path;
                    db.Database.Migrate();
                    //// Create
                    db.Add(new Blog { Url = "http://blogs.msdn.com/adonet", TestUrl = "http://blogs.msdn.com/adonet" });
                    db.SaveChanges();
                    // Read
                    var blog = db.Blogs
                        .OrderBy(b => b.BlogId)
                        .ToList().Select(b => b.TestUrl);
                    SQLite.ItemsSource = blog;
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}
