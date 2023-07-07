using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tesseract4Analyze
{
    internal static class Program
    {
        private static void Main()
        {
            // cfg
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            var cwd = AppDomain.CurrentDomain.BaseDirectory;
            var pluginSettingsPath = Path.Combine(cwd, "Tesseract4AnalyzeSettings.json");
            var pluginSettings = JsonConvert.DeserializeObject<PluginSettings>(File.ReadAllText(pluginSettingsPath));

            // using only 1 thread per current process for Tesseract, best efficiency for parallel batch processing according to online resources
            Environment.SetEnvironmentVariable("OMP_THREAD_LIMIT", "1", EnvironmentVariableTarget.Process);


            // get input
            var inputString = Console.ReadLine();
            var pluginInput = JsonConvert
                .DeserializeObject<List<ImageFile>>(inputString ?? throw new InvalidOperationException());
            var pluginOutput = new PluginOutput
            {
                CustomProperties = new List<CustomProperty>()
            };

            var tess = new TessEngine(
                Path.Combine(cwd, pluginSettings.TessdataPath),
                pluginSettings.LanguageString
            );


            // iterate over files
            foreach (var imageFile in pluginInput)
                try
                {
                    var task = Task.Run(() => tess.ReadTextFromImage(imageFile.FilepathConverted));

                    if (!task.Wait(TimeSpan.FromSeconds(pluginSettings.TimeOut)))
                    {
                        Console.Error.WriteLine($"TessEngine timed out while processing {imageFile.FilepathConverted}");
                        continue;
                    }

                    pluginOutput.CustomProperties.Add(new CustomProperty
                    {
                        Id = 0,
                        Sha1 = imageFile.Sha1Hex,
                        Value = task.Result
                    });
                }
                catch (IOException)
                {
                    // targeting "not an image file error" thrown by TesseractOCR.Pix.Image.LoadFromFile:
                    // That's fine as some of these files can't be read as an Image..
                }

            // write output
            var outputString = JsonConvert.SerializeObject(pluginOutput);
            Console.WriteLine(outputString);
        }
    }
}