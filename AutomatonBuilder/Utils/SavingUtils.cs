using AutomatonBuilder.Entities.Contexts;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutomatonBuilder.Utils
{
    public class SavingUtils
    {
        public static void SaveAsPng(MainEditingScreen host, string filePath)
        {
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

            host.MainToolBar.Visibility = Visibility.Visible;
        }

        public static void SaveAsEaf(MainEditingScreen mainWindow, string filePath)
        {
            JsonContext contextToSave = new JsonContext(mainWindow.context);

            File.WriteAllText(filePath, JsonConvert.SerializeObject(contextToSave, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            }));
            mainWindow.ChangeTitle(filePath.Split('\\')[^1].Split('.')[0]);
            MessageBox.Show("The model has been saved successfully.", "Saved successfully", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static AutomatonContext LoadContextFromFile(MainEditingScreen mainWindow, string filePath)
        {
            string jsonString = File.ReadAllText(filePath);

            JsonContext savedContext = JsonConvert.DeserializeObject<JsonContext>(jsonString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            })!;
            mainWindow.Title = filePath.Split('\\')[^1].Split('.')[0];

            return savedContext.ToContext(mainWindow.MainCanvas, mainWindow);
        }
    }
}
