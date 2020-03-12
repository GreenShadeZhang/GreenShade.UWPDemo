using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GreenShade.Wpf.DrawImageByGDI
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

        private void GetImg_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                using (MemoryStream ms = new MemoryStream(File.ReadAllBytes("E:\\21.jpg")))
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(ms))
                    {
                        using (Graphics g = Graphics.FromImage(image))
                        {
                            g.DrawString("天下第一" + i, new Font("宋体", 34), new SolidBrush(System.Drawing.Color.Black), 0, 0);
                            image.Save("E:\\22" + i + ".jpg");
                            g.Dispose();
                        }
                        image.Dispose();
                    }
                    ms.Dispose();
                }
            }

        }
    }
}
