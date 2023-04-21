# Tesseract4Analyze

![build](https://img.shields.io/github/actions/workflow/status/con-web-adipp/Tesseract4Analyze/build.yml)
![release](https://img.shields.io/github/v/release/con-web-adipp/Tesseract4Analyze)
![license](https://img.shields.io/github/license/con-web-adipp/Tesseract4Analyze)

Tesseract4Analyze is a Griffeye Analyze DI Pro plugin that provides optical character recognition (OCR) for images.
The plugin is based on the [Tesseract OCR Engine](https://github.com/tesseract-ocr/tesseract) and is available under MIT license.


## First steps

### System requirements


- Win64
- .NET Framework 4.8
- [Griffeye Analyze DI Pro](https://www.griffeye.com/analyze-di/)


### Download and installation

The latest release is available for download on the [release page of this repository](https://github.com/con-web-adipp/Tesseract4Analyze/releases).


Install the plugin by selecting ``Tesseract4Analyze_[Version].appkg`` from ``File -> Settings -> Plugins -> Install`` within Griffeye Analyze DI Pro.

The plugin is ready to use immediately (e.g. via ``Case Data -> Rescan against Plugins -> Tesseract4Analyze``). English and German are set as default languages for detection (see also: Language pack configuration).

### Basic usage

The plugin adds a column named "OCR text" to the grid, which can then be searched using column filters ("contains").
For example, if you filter by "account", you will get any image which contains the string "account" in the OCR-read text.


## Configuration

### Performance configuration


The plugin is designed to be run in parallel. This means that Analyze DI Pro calls the process several times at once.
For optimal CPU utilization, there should be exactly as many processes running concurrently as there are (logical) cores in the CPU.

See: ``File -> Settings -> Plugins -> Tesseract4Analyze -> Edit Plugin Settings -> Max Instances`` for configuration (default: 8).


### Languages

Tesseract natively supports over 100 languages and scripts (including Latin Antiqua, Fraktur, Devanagari (Indian script), Chinese, Arabic, Greek, Hebrew, Cyrillic and more).

The following languages and scripts are already included in this plugin package by default:

Languages:
- Arabic
- German
- English
- French
- German
- Italian
- French
- Polish 
- Portuguese
- Russian
- Spanish
- Turkish

Fonts:
- Arabic
- Cyrillic
- Georgian
- Greek
- Latin

Other languages and scripts provided by Tesseract can be easily downloaded and integrated into the plugin (see below).

### Language package configuration

The languages used for detection are specified in a json file:

```
C:\ProgramData\Griffeye Technologies\Griffeye Analyze\Data\Plugins\Tesseract4Analyze\Tesseract4AnalyzeSettings.json
```

The contents of the file look like this:
```json
{
  "TessdataPath": "tessdata-fast",
  "Languages": [
    "deu",
    "eng"
  ]
}
```

Languages can be added to the configuration by adding the key for the respective language package to the list:

```json
{
  "TessdataPath": "tessdata-fast",
  "Languages": [
    "deu",
    "eng",
    "fra",
    "ita",
    "por",
    "spa"
  ]
}
```

This configuration now contains the language packages German, English, French, Italian, Portuguese and Spanish.

The language packs themselves can be found at
```
C:\ProgramData\Griffeye Technologies\Griffeye Analyze\Data\Plugins\Tesseract4Analyze\tessdata-fast\
```

They follow the naming convention [country code].traineddata, e.g. deu.traineddata for the German language package.

The script packages can be found at
```
C:\ProgramData\Griffeye Technologies\Griffeye Analyze\Data\Plugins\Tesseract4Analyze\tessdata-fast\scripts\
```
They follow a slightly different naming convention: [script name].traineddata, for example: Cyrillic.traineddata.

It is possible to use both scripts and language packages for detection:

```json
{
  "TessdataPath": "tessdata-fast",
  "Languages": [
    "deu",
    "scripts/Arabic"
  ]
}
```

### Additional language packages


Additional language or script packages can be downloaded from [https://github.com/tesseract-ocr/tessdata_fast](https://github.com/tesseract-ocr/tessdata_fast) and just need to be copied to the appropriate folder (tessdata-fast, or tessdata-fast/scripts).


## See also

- [https://github.com/tesseract-ocr/tesseract](https://github.com/tesseract-ocr/tesseract)
- [https://github.com/Sicos1977/TesseractOCR](https://github.com/Sicos1977/TesseractOCR)
- [https://www.griffeye.com/analyze-di/](https://www.griffeye.com/analyze-di/)
