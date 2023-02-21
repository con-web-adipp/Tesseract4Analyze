using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test()
        {
            const string projectName = "Tesseract4Analyze";

            var cwd = AppDomain.CurrentDomain.BaseDirectory;
            var binPath = Path.GetFullPath(cwd +
                                           $"../../../../{projectName}/bin/x64/Release/{projectName}.exe");
            
            var picPath = Path.GetFullPath(cwd + "/testocr.png").Replace(@"\", "/");
            var inputString =
                $@"[{{'Sha1Hex': 'D3B255E3C9F31136767C0380EC2E5B3D7382C75C', 'FilepathConverted': '{picPath}'}}]";
            
            
            string outputString = null;
            string errorString = null;


            using (var process = new Process())
            {
                process.StartInfo.FileName = binPath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;


                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;


                var inputWriterTask = new Task(() =>
                {
                    using (var w = new StreamWriter(process.StandardInput.BaseStream))
                    {
                        w.WriteLine(inputString);
                    }
                });

                var outputReaderTask = new Task(() =>
                {
                    using (var r = new StreamReader(process.StandardOutput.BaseStream))
                    {
                        outputString = r.ReadToEnd();
                    }
                }, TaskCreationOptions.LongRunning);

                var errorReaderTask = new Task(() =>
                {
                    using (var r = new StreamReader(process.StandardError.BaseStream))
                    {
                        errorString = r.ReadToEnd();
                    }
                });

                //Start the process
                process.Start();

                inputWriterTask.Start();
                inputWriterTask.Wait();

                errorReaderTask.Start();
                outputReaderTask.Start();
                process.WaitForExit();
                errorReaderTask.Wait();
                outputReaderTask.Wait();
            }

            const string validString =
                    "{\"CustomProperties\":[{\"Id\":0,\"Sha1\":\"D3B255E3C9F31136767C0380EC2E5B3D7382C75C\"," +
                    "\"Value\":\"This is a lot of 12 point text to test the\\n\\nocr code and see if it works on " +
                    "all types\\n\\nof file format.\\n\\nThe quick brown dog jumped over the\\n\\nlazy fox. The " +
                    "quick brown dog jumped\\n\\nover the lazy fox. The quick brown dog\\n\\njumped over the lazy fox. " +
                    "The quick\\n\\nbrown dog jumped over the lazy fox.\\n\"}]}\r\n";

            Console.WriteLine(errorString);
            Assert.AreEqual(validString, outputString);
        }
    }
}
