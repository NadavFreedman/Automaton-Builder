using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutomatonBuilder.Utils
{
    public class SavingUtils
    {
        public static void SaveAsPng(MainWindow host, string filePath)
        {
            //Hide the tool bar
            host.MainToolBar.Visibility = Visibility.Hidden;

            //Save the screen to bitmap
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)host.MainCanvas.ActualWidth, (int)host.MainCanvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(host.MainCanvas);
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            MemoryStream stream = new MemoryStream();
            png.Save(stream);
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
            image.Save(filePath, ImageFormat.Png);
            MessageBox.Show("The model has been saved as an image successfully.", "Saved successfully", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void SaveAsAut(MainWindow mainWindow, string filePath)
        {
            var savedJson = JsonConvert.SerializeObject(mainWindow.context);
            File.WriteAllText(filePath, savedJson);
            MessageBox.Show("The model has been saved successfully.", "Saved successfully", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
