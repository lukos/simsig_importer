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
There is a sample spreadsheet in the project that can be used as a starting point. There is some flexibility as described below but the important points are that the individual sheets are given the specific names "Train Types", "Timetable" and "Seed Groups" although only Timetable is mandatory. Any other sheets, that you might be using to work things out, will simply be ignored.

We will probably look at making none of the sheets mandatory in the future to allow you to layer various documents on top of each other, e.g. you might have a standard list of train types which instead of copying and pasting you could import first before then importing various iterations of timetables.

## Seed groups
Seed groups are very straight-forward. Add a unique id to tie it to a working on the timetable sheet and then just fill in the mandatory start time and the optional title.

## Train types
Train Types are also quite self-explanatory but don't move or change the orange text on the train types sheet, which is used to create the dropdowns for the train types tables and which needs to match text in the importer to correctly import the train. You can use whichever IDs you want for the trains as long as they match what you specify in the "power" row of the timetable sheet. These IDs will not be used in the exported Simsig timetable, these will be replaced with unique ids in the correct format for simsig.

If you need more rows of dropdowns on the trains page, simply find a good row and copy it downwards, which will copy the dropdowns.

Note we cannot easily support multiple speed classes or electrifications, so you will have to pick a single one and then edit it in simsig afterwards.

## Dwell times
These are currently not supported in the importer

## Timetables
The timetable sheet is where you describe your workings. The sample shows you roughly how these are laid out. Note that not all of the boxes are currently used, we are instead aiming to get the basics working first and then adding the others later.

The layout is from left-to-right with a single row of workings, which follows what most working timetables do, which should make it easier to copy data into the spreadsheet. If you need to include up and down workings, then you will need to create two spreadsheets, importing the one with the train types etc. first and then importing the second timetable which will contain the other timetables.

You need to have the row headings in column A on the timetable but you are permitted to use any number of blank rows between them, except that the locations cannot have any blank lines in them. The special row headings are:

* Headcode - the ID of the train e.g. 1H10
* Power - the ID of the train type used for this service at entry/start
* Days - this will allow us to generate timetables for each day automatically and needs to follow standard naming like SO or MSX
* Seeds - If set is the location to seed the train at. This will be simulation specific and might be a singal number like S39

The locations go across two columns, A and B. Column A is the TIPLOC code for the location that you are timing the train at. The Simsig manuals will describe which timing points are mandatory for validation purposes which might or might not line up with timings in the working timetable. For example, for Wolverhampton, Bushbury junction is mandatory in the simulation but not in the WTT. Dudley port on the other hand appears in both. You can find a list of TIPLOC codes [here](http://www.railwaycodes.org.uk/crs/crs0.shtm). I do NOT know if all locations in the simulations have the correct TIPLOC codes, if yours doesn't import correctly, you might need to export a known-good timetable and check the code that is written into the XML.

Locations with a # at the beginning are entry locations. The first one of these that has a time in it will be used as the entry point and time for that working, which doesn't appear in the locations list.

Column B can be used for a friendly name like in the sample, except for 5 special codes "Path", "Plat", "Line", "Arr" and "Dep". The first 3 are optional but they must appear in the order specified if used. Path, plat and line are simulation and location specific e.g. Wolverhampton station does not have any paths or lines set to choose from. They could be e.g. 3 for a platform but could also be DS, Down Slow, 1A or whatever is relevant to your particular simulation.

"Arr" and "Depart" are the arrival and departure times. There are a number of ways these can be used:
* Both can simply contain times, in which case they will be interpreted as a normal station-stop type entry
* If arr has a time, dep can contain an activity (see below for codes). It cannot contain a code and a departure time currently so this might need to be improved in the future, otherwise the train will need editing in simsig later.
* You can include a previous code in the arr box e.g. P:5V41 and then a departure time. The previous code won't be used anywhere in the timetable but is helpful for the importer to distinguish between an originating working and a passing service
* If arr is empty and dep has a time, then it will become a simple passing time

### Single line arrivals/departures
A common trick in working timetables to save space is to have a single line for a train that arrives and departs at a station but where there is never/rarely any deviation from the timings. These are highlighted in bold and if encountered in the importer, the time will be used for the departure time and the arrival time will be set to 1 minute earlier, which is fairly standard for normal station stops.

### Activities
The following activites can be used in the Dep row. Multiple ones can be used separated by a space and they follow the form of &lt;code>:&lt;associated train>

Codes
* N - Next train
* J - Join
* DNR - Divides (new rear)
* DNF - Divides (new front)
* DER - Detach engine (rear)
* DEF - Detach engine (front)
* DCR - Drop coaches (rear)
* DCF - Drop coaches (front)

### Difficult workings
Some of the more complex features of a timetable are not easy to handle in a spreadsheet. For example, a train might logically reverse somewhere and go from Up to Down or vice-versa. Although this might be physically possible in the simulation (e.g. going round a chord onto another line), these cannot be represented easily in the working timetable and usually appear as multiple entries, one for the up part(s) and another for the down part(s).

The importer has no special knowledge of these unusual workings so it can only work from top to bottom when building locations. If the locations that are mandatory allow your working to physically change direction while still moving top to bottom of the timeable, then the working will import fine. If it would effectively have to go downwards and then move back up, even if you use the correct times, this won't work. Instead, you will need to either enter it as two separate workings like 1M10-1 and 1M10-2 (which will only work if you have a location that the service can stop and step its ID) or you will need to manually tweak them after importing.

For example, the first Up locations I am using for Wolverhampton are these:

* Stafford - 1
* Penkridge - 2
* Littleton - 3
* Four ashes - 4
* Bushbury Jn - 5
* Cosford - 7
* Albrighton
* Codsall
* Bilbrook
* Oxley - 6

If a train comes from stafford but turns at Bushbury junction towards Oxley and Cosford (the location order as numbered above), this wouldn't be possible since Oxley is a mandatory timing point and appears below Cosford, which is the exit point of the simulation. The only trains that do this route are freight and there is no stopping point where their code could change so I will simply need to edit them after importing and add any of the remaining locations that I can't import.

# Contributing
## Developers
Initially, there is unlikely to be much need to extend the functionality but in the first instance, please either message me in the Simsig forum (username lukebriner) and I will either open the discussion to others to see what they think and/or then open an issue on Github. 

If you are a .Net programmer and want to offer some development time, please let me know via DM or via a github issue if there is one you could work on. I can't promise that I will accept all Pull Requests so please check before starting work so that I can see some of your code. This project won't suit people who want to hack around with something that they don't fully understand, only experienced dotnet devs need apply. Please don't make me have awkward conversations - some people can be too helpful!

## Feature suggestions
Note that people often offer what they think are helpful suggestions for features but the amount of work and risk involved with every change means that we might refuse a feature that we don't think will benefit many users or something that will be hard to keep simple. In most cases, it is easy enough to work around any shortcomings by post-editing the timetable in SimSig itself. It will always be easiest to start the conversation on the SimSig forum so we can get consensus on what is and what isn't a valuable change.

You can also look on the issues tab to see if the feature is already raised

## Bug reports
If something crashes, it will always be a bug so please feel free to raise a Github issue. You will ideally need to attach the timetable you are trying to import and describe exactly what you were doing and at what point it crashed. If you don't I won't spend much time trying to recreate it and old bugs might eventually get closed.

If it is something that doesn't appear to work as expected, please ask on the forum first so we can ascertain whether it is a documentation problem or there needs to be a code change to fix/change how something works.