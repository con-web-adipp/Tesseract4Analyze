using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TesseractOCR;
using TesseractOCR.Enums;
using TesseractOCR.Pix;

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

            // setting up engine and output


            using (var engine = new Engine(
                       Path.Combine(cwd, pluginSettings.TessdataPath ),
                       pluginSettings.LanguageString))
            {
                engine.SetVariable("debug_file","/dev/null"); // suppress redundant logger messages from tesseract BaseAPI
                engine.SetVariable("tessedit_do_invert", 0); // improves speed according to the docs

                // iterate over files
                foreach (var imageFile in pluginInput)
                    try
                    {
                        var img = Image.LoadFromFile(imageFile.FilepathConverted);
                        var output = engine.Process(img, PageSegMode.SparseText);
                        var outputText = output.Text;
                        img.Dispose();
                        output.Dispose();


                        if (outputText != string.Empty)
                            pluginOutput.CustomProperties.Add(new CustomProperty
                            {
                                Id = 0,
                                Sha1 = imageFile.Sha1Hex,
                                Value = outputText
                            });
                    }
                    catch (IOException)
                    {
                        // targeting "not an image file error" thrown by TesseractOCR.Pix.Image.LoadFromFile:
                        // That's fine as some of these files can't be read as an Image..
                    }
            }

            // write output
            var outputString = JsonConvert.SerializeObject(pluginOutput);
            Console.WriteLine(outputString);
        }
    }
}