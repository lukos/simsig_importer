# Contributing
## Developers
Initially, there is unlikely to be much need to extend the functionality but in the first instance, please either message me in the Simsig forum (username lukebriner) and I will either open the discussion to others to see what they think and/or then open an issue on Github. 

If you are a .Net programmer and want to offer some development time, please let me know via DM or via a github issue if there is one you could work on. I can't promise that I will accept all Pull Requests so please check before starting work so that I can see some of your code and make sure you can create something of suitable quality before you spend time and then get turned down. This project won't suit people who want to hack around with something that they don't fully understand, only experienced dotnet devs need apply! 

## Feature suggestions
Note that people often offer what they think are helpful suggestions for features but the amount of work and risk involved with every change means that we might refuse a feature that we don't think will benefit many users or something that will be hard to keep simple. In most cases, it is easy enough to work around any shortcomings by post-editing the timetable in SimSig itself. It will always be easiest to start the conversation on the SimSig forum so we can get consensus on what is and what isn't a valuable change.

You can also look on the issues tab to see if the feature is already raised

## Bug reports
If something crashes, it will always be a bug so please feel free to raise a Github issue. You will ideally need to attach the timetable you are trying to import and describe exactly what you were doing and at what point it crashed. If you don't I won't spend much time trying to recreate it and old bugs might eventually get closed.

If it is something that doesn't appear to work as expected, please ask on the forum first so we can ascertain whether it is a documentation problem or there needs to be a code change to fix/change how something works.

## Standard Changes
* Adding a new simulation. These are hard-coded in Form1.cs `PopulateSimDropdown` but note that the values need to match what SimSig expects in the id of the timetable. Currently these only set the value of the XML ID attribute.
* New import rows from the spreadsheet. The `SpreadsheetHelper` is the main class for importing. In general, we should make rows optional and use some code in `ProcessTimetable` to work out which row in the sheet matches what you want (or not) and then store that for later. If the row is optional, you will need to check this in `ProcessTimetableColumn` before attempting to read anything. GetCellValue() is the generic method that can find cell contents by Excel address e.g. A1