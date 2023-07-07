using System.Collections.Generic;

namespace Tesseract4Analyze
{
    public class PluginSettings
    {
        public string TessdataPath { get; set; }
        public List<string> Languages { get; set; }
        public int TimeOut { get; set; }
        public string LanguageString => string.Join("+", Languages);
    }
}