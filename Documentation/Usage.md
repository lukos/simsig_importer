# Usage instructions
There is a sample spreadsheet in the project that can be used as a starting point. There is some flexibility as described below but the important points are that the individual sheets are given the specific names "Train Types", "Timetable Up", "Timetable Down" and "Seed Groups" although none are mandatory mandatory. Any other sheets that you might be using to work things out, will simply be ignored.

You can import multiple spreadsheets which will help with trains that do not simply travel in the up or down direction but perhaps go around a chord and change. Other timetables need to be in the same general format but you can use whichever locations are necessary (and mandatory for simsig) to ensure that trains are always read from top to bottom since this is the order that their locations are added. Additional sheets can link to train types and seed groups previously imported but you cannot link a timetable to a train type that is not yet imported so import those first!

## Seed groups
Seed groups are very straight-forward. Add a unique id to tie it to a working on the timetable sheet and then just fill in the mandatory start time and the optional title.

## Train types
Train Types are also quite self-explanatory but don't move or change the orange text on the train types sheet, which is used to create the dropdowns for the train types tables and which needs to match text in the importer to correctly import the train. You can use whichever IDs you want for the trains as long as they match what you specify in the "power" row of the timetable sheet. These IDs will not be used in the exported Simsig timetable, these will be replaced with unique ids in the correct format for simsig.

If you need more rows of dropdowns on the trains page, simply find a good row and copy it downwards, which will copy the dropdowns.

Note we cannot easily support multiple speed classes or electrifications, so you will have to pick a single one and then edit it in simsig afterwards if necessary.

## Dwell times
These are currently not supported in the importer

## Timetables
The timetable sheet is where you describe your workings. The sample shows you roughly how these are laid out. Note that not all of the boxes are currently used, we are instead aiming to get the basics working first and then adding the others later.

The layout is from left-to-right with a single row of workings, which follows what most working timetables do, which should make it easier to copy data into the spreadsheet. If you need to include up and down workings, then you will need to create two spreadsheets, importing the one with the train types etc. first and then importing the second timetable which will contain the other timetables.

You need to have the row headings in column A on the timetable but you are permitted to use any number of blank rows between them, except that the locations cannot have any blank lines in them. The special row headings are:

* Headcode - the ID of the train e.g. 1H10
* Power - the ID of the "train type" used for this service at entry/start - if this is not found in the list of train types, it will have "custom" settings but these are set to 0 by default so will need editing later
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

### Engineering/Pathing Allowances
You can add engineering or pathing allowances for a location really easily. Add another row underneath the location you want to add allowances for and make sure there is a code in the first column so there are no gaps in the list of locations. It doesn't actually matter what is in column 1 but you might as well copy the location code from above. In the blank row, you can use bracket numbers like (2) to indicate a pathing allowance of 2 minutes and/or a square bracketed [4] to indicate an engineering allowance. There is an example in the provided spreadsheet in the Up trains under 1V44.

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