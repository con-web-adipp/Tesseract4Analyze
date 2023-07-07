using TesseractOCR;
using TesseractOCR.Enums;
using TesseractOCR.Pix;

namespace Tesseract4Analyze
{
    public class TessEngine
    {
        private string TessDataPath;
        private string Languages;
        private Engine Engine;


        public TessEngine(string tessDataPath, string languages)
        {
            TessDataPath = tessDataPath;
            Languages = languages;
            Engine = new Engine(TessDataPath, Languages);

            Engine.SetVariable("debug_file", "/dev/null"); // suppress redundant logger messages from tesseract BaseAPI
            Engine.SetVariable("tessedit_do_invert", 0); // improves speed according to the docs
        }
        
        public string ReadTextFromImage(string imageFilePath)
        {
            var img = Image.LoadFromFile(imageFilePath);
            var output = Engine.Process(img, PageSegMode.SparseText);
            var outputText = output.Text;
            img.Dispose();
            output.Dispose();
            return outputText;
        }
    }
}