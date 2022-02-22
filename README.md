# Simsig Importer
.Net Desktop Application for converting Excel simplified timetables into Simsig timetables

# Introduction
If you are creating your own timetables in [SimSig](https://www.simsig.co.uk/), the task quickly becomes onerous due to the excessive amount of UI interaction required to import a timetable that you are likely to have developed separately in something like a spreadsheet, which can more closely mirror a working timetable document.

The motivation for this application is to provide the means to create timetables in a simple Excel spreadsheet format, which can then be imported into a SimSig format timetable directly. Any other tweaks can be made once the timetable is imported into Simsig using the GUI since not all functions are exposed to the importer for complexity reasons.

# Licence
This software is licensed under the GNU General Public License v3 to allow you freedom to use and/or modify the software freely but with the requirement that you allow any changes to be made available along with an equivalent licence. Note that simsig software is proprietary and licenced by Hitachi Information and Control Systems Europe Limited to Cajon Software Ltd. I have no personal link to either Hitachi or Cajon, this is simply a hobby application developed with the knowledge of but not the official sanction of either Hitachi or Cajon.

I will endeavour to keep this up to date with any changes to timetable formats but without any direct access to source code, I can only do this with the helpful assistance of SimSig forum users.

# Installation
## Windows installer
To run the software, you will need to get the latest production release from the [releases](https://github.com/lukos/simsig_importer/releases) page (the one nearest the top). Any releases that are marked alpha or beta might contain new code that has not been fully tested. You are welcome to use those releases for features that are not available before then but with the usual caveat that they might not work properly yet. If you do not need any special new feature, just take the latest non-alpha/beat version.

The MSI is the installer that you need for Windows. The application will need the .Net framework 4.7.2 runtime to be installed but this should already be installed on newer versions of windows. If not, you can download it from [here](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net472-web-installer).

## Source code
If you want to build the source and run the app locally for development or from curiosity, then you will need Visual Studio 2019+ and have the workload for Winform apps installed (called Windows Desktop in the installer).

Note that the initial development has been made for Wolverhampton and has not yet been tested on other simulations. It doesn't seem like there would be much difference across simulations except needing to know the correct timetable version and location (TIPLOC) codes. You can do this manually by creating a suitable timetable in Simsig, then saving it, renaming the saved file to .zip and then opening the SavedTimetable.xml file in a text editor. You can find codes for anything that you have created in your timetable but note that not all fields are exported if they are not set, to reduce the amount of data being written to disk.

## Spreadsheet formats
Currently, the importer only supports Office Open XML format, which pretty much means Excel. This is simply to avoid all of the various conversions that people might need to do to make it work from random other formats. You should be able to save most other spreadsheets into Excel "xlsx" format in order to use it for this. CSV format is not suitable for the richer formats used in the spreadsheet so it will never be supported.

# Usage instructions
[See here](https://github.com/lukos/simsig_importer/tree/main/Documentation/Usage.md)

# Contributing
[See here](https://github.com/lukos/simsig_importer/tree/main/Documentation/Developers.md)