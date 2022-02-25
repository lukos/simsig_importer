using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SimsigImporterLib.Helpers;
using SimsigImporterLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimsigImporterLib
{
    public class SpreadsheetHelper
    {
        private readonly Action<string> info;
        private readonly Action<string> warning;
        private readonly Action<string> error;

        private static Regex trainId = new Regex("^[A-Z0-9]{8}$", RegexOptions.Compiled);
        

        public SpreadsheetHelper(Action<string> info, Action<string> warning, Action<string> error)
        {
            this.info = info;
            this.warning = warning;
            this.error = error;
        }

        /// <summary>
        /// Imports a simplified timetable file into memory
        /// </summary>
        /// <param name="fileName">The filename to open</param>
        /// <param name="tt">The existing timetable to add to</param>
        /// <returns></returns>
        public SimSigTimetable Import(string fileName, SimSigTimetable tt)
        {
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fileName, false))
            {
                /////////////////////////////////
                //
                // Seed groups
                //
                /////////////////////////////////
                Sheet sgSheet = (Sheet)doc.WorkbookPart.Workbook.Sheets.ChildElements.FirstOrDefault(sheet => ((Sheet)sheet).Name == "Seed Groups");
                if (sgSheet != null)
                {
                    info("Parsing seed groups");
                    var groups = ProcessSeedGroups(doc, sgSheet);
                    if (groups == null)
                    {
                        error("Error processing seed groups tab");
                        return null;
                    }
                    tt.SeedGroups.AddRange(groups);
                }
                else
                {
                    warning("No seed groups found in spreadsheet, skipping");
                }

                /////////////////////////////////
                //
                // Train types
                //
                /////////////////////////////////
                Sheet trainsSheet = (Sheet)doc.WorkbookPart.Workbook.Sheets.ChildElements.FirstOrDefault(sheet => ((Sheet)sheet).Name == "Train Types");
                if (trainsSheet != null)
                {
                    info("Parsing train types");
                    var trainTypes = ProcessTrainTypes(doc, trainsSheet);
                    if (trainTypes == null)
                    {
                        error("Error processing train types tab");
                        return null;
                    }
                    tt.TrainCategories.AddRange(trainTypes);
                }
                else
                {
                    warning("No train types found in spreadsheet, all services will have custom settings");
                }

                /////////////////////////////////
                //
                // Up timetable
                //
                /////////////////////////////////
                Sheet ttSheet = (Sheet)doc.WorkbookPart.Workbook.Sheets.ChildElements.FirstOrDefault(sheet => ((Sheet)sheet).Name == "Timetable Up");
                if (ttSheet == null)
                {
                    warning("No Up timetable found in spreadsheet. The sheet needs to be called Timetable Up");
                }
                else
                {
                    var upTTs = ProcessTimetable(doc, ttSheet, tt);

                    if (upTTs == null)
                    {
                        warning("No timetables found in Up timetable sheet. Possible formatting error. Ignoring");
                    }
                    else
                    {
                        tt.Timetables.AddRange(upTTs);
                        info("Mapping timetables to their train type");
                        foreach (var working in upTTs)
                        {
                            if (tt.TrainCategories.Any(tc => tc.SheetId == working.Category))
                            {
                                var trainData = tt.TrainCategories.First(tc => tc.SheetId == working.Category);
                                working.Category = trainData.ID;
                                // Train data is copied across (denormalised) to the working even though it has the reference above
                                working.TrainLength = trainData.TrainLength;
                                working.AccelBrakeIndex = trainData.AccelBrakeIndex;
                                working.MaxSpeed = trainData.MaxSpeed;
                                working.SpeedClass = trainData.SpeedClass;
                                working.IsFreight = trainData.IsFreight;
                                working.CanUseGoodsLines = trainData.CanUseGoodsLines;
                                working.Electrification = trainData.Electrification;
                                working.StartTraction = trainData.Electrification;
                            }
                            else
                            {
                                warning($"Could not find train type for ID {working.Category}");
                            }
                        }
                    }
                }

                /////////////////////////////////
                //
                // Down timetable
                //
                /////////////////////////////////
                Sheet ttSheet2 = (Sheet)doc.WorkbookPart.Workbook.Sheets.ChildElements.FirstOrDefault(sheet => ((Sheet)sheet).Name == "Timetable Down");
                if (ttSheet2 == null)
                {
                    warning("No Down timetable found in spreadsheet. The sheet needs to be called Timetable Down");
                }
                else
                {
                    var downTt = ProcessTimetable(doc, ttSheet2, tt);

                    if (downTt == null)
                    {
                        warning("No timetables found in Down timetable sheet. Possible formatting error. Ignoring");
                    }
                    else
                    {
                        tt.Timetables.AddRange(downTt);
                        info("Mapping timetables to their train type");
                        foreach (var working in downTt)
                        {
                            if (tt.TrainCategories.Any(tc => tc.SheetId == working.Category))
                            {
                                var trainData = tt.TrainCategories.First(tc => tc.SheetId == working.Category);
                                working.Category = trainData.ID;
                                // Train data is copied across (denormalised) to the working even though it has the reference above
                                working.TrainLength = trainData.TrainLength;
                                working.AccelBrakeIndex = trainData.AccelBrakeIndex;
                                working.MaxSpeed = trainData.MaxSpeed;
                                working.SpeedClass = trainData.SpeedClass;
                                working.IsFreight = trainData.IsFreight;
                                working.CanUseGoodsLines = trainData.CanUseGoodsLines;
                                working.Electrification = trainData.Electrification;
                                working.StartTraction = trainData.Electrification;
                            }
                            else
                            {
                                warning($"Could not find train type for ID {working.Category}");
                            }
                        }
                    }
                }

                return tt;
            }
        }

        /// <summary>
        /// The timetable is the most complex part to read in but is generally one or more columns containing train descriptions with their
        /// locations and times. We can't encapsulate all of the complexity of simsig into the target spreadsheet but we will need to decode various
        /// directives like where we stop and where we just pass and also need to link any seed groups or power specifications to types already
        /// imported. If these are disconnected, we can make safe assumptions or add placeholders but we will need to warn the user if we do this
        /// so they can choose to fix/reimport or to fix it later in SimSig.
        /// </summary>
        /// <param name="doc">The workbook</param>
        /// <param name="ttSheet">The sheet with the timetable on it</param>
        /// <param name="tt">The existing tt containing data on seeding groups and train types</param>
        /// <returns></returns>
        private List<Timetable> ProcessTimetable(SpreadsheetDocument doc, Sheet ttSheet, SimSigTimetable tt)
        {
            WorkbookPart wbPart = doc.WorkbookPart;
            Worksheet worksheet = ((WorksheetPart)wbPart.GetPartById(ttSheet.Id)).Worksheet;
            var rows = worksheet.Descendants<Row>().ToList();

            // Work out which rows are which
            int headcodeRow = 0, powerRow = 0, seedsRow = 0, locationStartRow = 0, locationEndRow = 0, daysRow = 0, uniqueIdRow = 0;

            for(int row = 1; row <= rows.Count; ++row)
            {
                if ( GetCellValue(wbPart,worksheet, $"A{row}").IsMissing() )
                {
                    if(locationStartRow != 0)
                    {
                        // The first blank entry after we have started the locations is the last location row and we don't care after that!
                        locationEndRow = row - 1;
                        break;
                    }
                    continue;
                }
                if (GetCellValue(wbPart, worksheet, $"A{row}") == "Headcode")
                {
                    headcodeRow = row; continue;
                }
                if (GetCellValue(wbPart, worksheet, $"A{row}") == "UniqueId")
                {
                    uniqueIdRow = row; continue;
                }
                if (GetCellValue(wbPart, worksheet, $"A{row}") == "Power")
                {
                    powerRow = row; continue;
                }
                if (GetCellValue(wbPart, worksheet, $"A{row}") == "Days")
                {
                    daysRow = row; continue;
                }
                if (GetCellValue(wbPart, worksheet, $"A{row}") == "Seeds")
                {
                    seedsRow = row; continue;
                }
                if (GetCellValue(wbPart, worksheet, $"A{row}").StartsWith("#"))
                {
                    if (locationStartRow == 0)
                    {
                        locationStartRow = row;
                    }
                    continue;
                }
            }

            if ( headcodeRow == 0 || powerRow == 0 || seedsRow == 0 || locationStartRow == 0 || locationEndRow == 0 )
            {
                error("Cannot find all of the rows in the Timetable sheet");
                return null;
            }


            // Find the first working column, must be in the first 10 columns, otherwise that will be ridiculous
            var workingColumn = 1;
            for(int firstCol = 3; firstCol < 10; ++firstCol)
            {
                if (GetCellValue(wbPart, worksheet, $"{firstCol.ToExcelColumn()}{headcodeRow}").IsPresent())
                {
                    workingColumn = firstCol;
                    break;
                }
            }
            if ( workingColumn == 1)
            {
                warning("Cannot find a working in the Headcode row of Timetable sheet");
                return null;
            }

            var timetables = new List<Timetable>();
            var columnCount = worksheet.Descendants<Row>().Max(r => r.ChildElements.Count);
            for (int workCol = workingColumn; workCol <= columnCount; ++workCol)
            {
                // Attempt to process a timetable in each column on the sheet. Note that it is possible there are extra blank columns
                // and others with random stuff so just look for headcode content
                if (GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{headcodeRow}").IsMissing())
                {
                    continue;
                }
                var timetable = ProcessTimetableColumn(wbPart, worksheet, workCol, headcodeRow, powerRow, seedsRow, locationStartRow, locationEndRow, daysRow, uniqueIdRow);
                if ( timetable != null)
                {
                    timetables.Add(timetable);
                }
            }

            if (timetables.Count == 0)
            {
                error("Failed to add any timetables from the Timetable sheet");
                return null;
            }

            return timetables;
        }

        /// <summary>
        /// Attempts to parse an excel column to create a timetable object
        /// </summary>
        private Timetable ProcessTimetableColumn(WorkbookPart wbPart, Worksheet worksheet, int workCol, int headcodeRow, int powerRow, int seedRow, int locationStartRow, int locationEndRow, int daysRow, int uniqueIdRow)
        {
            var tt = new Timetable();
            try
            {
                tt.ID = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{headcodeRow}");
                tt.Category = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{powerRow}");     // We will relink these after loading them all
                tt.SeedPoint = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{seedRow}");

                if ( uniqueIdRow > 0 )
                {
                    tt.UID = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{uniqueIdRow}");
                }

                var days = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{daysRow}");
                if ( Validators.VerifyDayCode(days))
                {
                    tt.Days = days;
                }

                for (int row = locationStartRow; row <= locationEndRow; ++row)
                {
                    var cellContents = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{row}");
                    if (cellContents.IsMissing())
                    {
                        continue;
                    }

                    var location = GetCellValue(wbPart, worksheet, $"A{row}");
                    if (tt.SeedPoint.IsMissing() && tt.EntryPoint.IsMissing() )
                    {
                        // # is a marker to help sanitize imported data. It is only used for importing and should be discarded
                        if (location.StartsWith("#"))
                        {
                            // First entry location is not a "trip" but the entry time and point
                            tt.EntryPoint = location.Substring(1);
                            tt.DepartTime = cellContents.ToSimsigTime();
                            continue;
                        }
                    }
                    var specialCode = GetCellValue(wbPart, worksheet, $"B{row}");
                    var specialCodes = new List<string> { "Path", "Plat", "Arr", "Dep", "Line" };
                    if ( !specialCodes.Contains(specialCode))
                    {
                        // Just a normal trip entry
                        // bold means that it arrives 1 minute earlier than the time in the cell
                        var cell = GetCell(worksheet, $"{workCol.ToExcelColumn()}{row}");
                        var isBold = cell.StyleIndex == 11;

                        var trip = new Trip
                        {
                            Location = location.StartsWith("#") ? location.Substring(1) : location,
                            ArrTime = isBold ? (int?)(cellContents.ToSimsigTime() - 60) : null,
                            DepPassTime = cellContents.ToSimsigTime(),
                            IsPassTime = !isBold
                        };
                        // Check for allowances
                        var nextCell = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{row+1}");
                        if ( HasAllowances(trip, nextCell))
                        { 
                            ++row;
                        }

                        tt.Trips.Add(trip);
                    }
                    else
                    {
                        var trip = new Trip
                        {
                            Location = GetCellValue(wbPart, worksheet, $"A{row}")
                        };
                        // Order of special columns must be Path, Plat, Line, Arr, Dep (although you only need arr dep to be present)
                        if ( specialCode == "Path" )
                        {
                            trip.Path = cellContents;   // No checking
                            ++row;
                            cellContents = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{row}");
                            specialCode = GetCellValue(wbPart, worksheet, $"B{row}");
                        }
                        if (specialCode == "Plat")
                        {
                            trip.Platform = cellContents;   // No checking, could be e.g. 1A
                            ++row;
                            cellContents = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{row}");
                            specialCode = GetCellValue(wbPart, worksheet, $"B{row}");
                        }
                        if (specialCode == "Line")
                        {
                            trip.Line = cellContents;   // No checking
                            ++row;
                            cellContents = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{row}");
                            specialCode = GetCellValue(wbPart, worksheet, $"B{row}");
                        }
                        if (specialCode == "Arr")
                        {
                            // Previous is a way of keeping tidy records in our simplifier but is not needed for simsig
                            if (!cellContents.StartsWith("P"))
                            {
                                trip.ArrTime = cellContents.ToSimsigTime();
                            }
                            trip.IsPassTime = !cellContents.IsPresent();
                            ++row;
                            cellContents = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{row}");
                            specialCode = GetCellValue(wbPart, worksheet, $"B{row}");
                        }
                        if(specialCode == "Dep")
                        {
                            trip.Activities = TryGetActivities(cellContents);
                            if (trip.Activities == null )
                            {
                                trip.DepPassTime = cellContents.ToSimsigTime();
                            }
                            // Check for allowances
                            var nextCell = GetCellValue(wbPart, worksheet, $"{workCol.ToExcelColumn()}{row + 1}");
                            if (HasAllowances(trip, nextCell))
                            {
                                ++row;
                            }
                        }
                        tt.Trips.Add(trip);
                    }
                    
                }
            }
            catch(Exception)
            {
                warning($"Cannot read timetable column {workCol.ToExcelColumn()}. Ignoring");
                return null;
            }
            return tt;
        }

        /// <summary>
        /// Writes the given location data to the template spreadsheet
        /// </summary>
        /// <param name="path">The path to the new file</param>
        /// <param name="data">The location data to write</param>
        public void WriteLocations(string path, Template data)
        {
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(path, true))
            {
                // Find the two timetable tables and the "INSERTHERE" text and write the locations in opposite order
                Sheet ttSheet1 = (Sheet)doc.WorkbookPart.Workbook.Sheets.ChildElements.FirstOrDefault(sheet => ((Sheet)sheet).Name == "Timetable Up");
                Sheet ttSheet2 = (Sheet)doc.WorkbookPart.Workbook.Sheets.ChildElements.FirstOrDefault(sheet => ((Sheet)sheet).Name == "Timetable Down");
                if (ttSheet1 == null || ttSheet2 == null)
                {
                    error("Could not find one or both timetable sheets in template, exiting");
                    return;
                }

                WorkbookPart wbPart = doc.WorkbookPart;
                var shareStringPart = wbPart.GetPartsOfType<SharedStringTablePart>().First();


                WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(ttSheet1.Id);
                Worksheet worksheet = wsPart.Worksheet;
                var rows = worksheet.Descendants<Row>().ToList();

                // Work out which row to insert
                uint insertRow = 0;

                for (uint row = 1; row <= rows.Count; ++row)
                {
                    var contents = GetCellValue(wbPart, worksheet, $"A{row}");
                    if (contents.IsPresent() && contents == "INSERTHERE")
                    {
                        insertRow = row;
                    }
                }

                if ( insertRow == 0 )
                {
                    error("No insert point in up timetable sheet. Exiting");
                    return;
                }

                var currentRow = insertRow;
                // Up direction
                for (uint index = 0; index < data.Locations.Length; ++index)
                {
                    var location = data.Locations[index];
                    if (location.Direction == "Down")
                        continue;

                    var code = location.UpEntry ? "#" + location.Code : location.Code;

                    AddLocationData(shareStringPart, wsPart, code, "A", currentRow);

                    if (!location.ArrDep)
                    {
                        // Arr/dep is the only thing that splits the lines so if you don't have it, write the location name
                        // and move on
                        AddLocationData(shareStringPart, wsPart, location.Name, "B", currentRow);
                        ++currentRow;
                        continue;
                    }

                    if ( location.Path )
                    {
                        AddLocationData(shareStringPart, wsPart, "Path", "B", currentRow);
                        ++currentRow;
                        AddLocationData(shareStringPart, wsPart, code, "A", currentRow);
                    }

                    if (location.Plat)
                    {
                        AddLocationData(shareStringPart, wsPart, "Plat", "B", currentRow);
                        ++currentRow;
                        AddLocationData(shareStringPart, wsPart, code, "A", currentRow);
                    }

                    if (location.Line)
                    {
                        AddLocationData(shareStringPart, wsPart, "Line", "B", currentRow);
                        ++currentRow;
                        AddLocationData(shareStringPart, wsPart, code, "A", currentRow);
                    }

                    // We have arr/dep if we get this far so should be easy now!
                    AddLocationData(shareStringPart, wsPart, "Arr", "B", currentRow);
                    ++currentRow;
                    AddLocationData(shareStringPart, wsPart, code, "A", currentRow);
                    AddLocationData(shareStringPart, wsPart, "Dep", "B", currentRow);
                    ++currentRow;
                }

                // Now for the down timetable sheet. Same thing but in the reverse order and logic *very* slightly different
                wsPart = (WorksheetPart)wbPart.GetPartById(ttSheet2.Id);
                worksheet = wsPart.Worksheet;
                rows = worksheet.Descendants<Row>().ToList();

                // Work out which row to insert
                insertRow = 0;

                for (uint row = 1; row <= rows.Count; ++row)
                {
                    var contents = GetCellValue(wbPart, worksheet, $"A{row}");
                    if (contents.IsPresent() && contents == "INSERTHERE")
                    {
                        insertRow = row;
                    }
                }

                if (insertRow == 0)
                {
                    error("No insert point in down timetable sheet. Exiting");
                    return;
                }

                currentRow = insertRow;
                // Up direction
                for (var index = data.Locations.Length - 1; index >= 0; --index)
                {
                    var location = data.Locations[index];
                    if (location.Direction == "Up")
                        continue;

                    var code = location.DownEntry ? "#" + location.Code : location.Code;

                    AddLocationData(shareStringPart, wsPart, code, "A", currentRow);

                    if (!location.ArrDep)
                    {
                        // Arr/dep is the only thing that splits the lines so if you don't have it, write the location name
                        // and move on
                        AddLocationData(shareStringPart, wsPart, location.Name, "B", currentRow);
                        ++currentRow;
                        continue;
                    }

                    if (location.Path)
                    {
                        AddLocationData(shareStringPart, wsPart, "Path", "B", currentRow);
                        ++currentRow;
                        AddLocationData(shareStringPart, wsPart, code, "A", currentRow);
                    }

                    if (location.Plat)
                    {
                        AddLocationData(shareStringPart, wsPart, "Plat", "B", currentRow);
                        ++currentRow;
                        AddLocationData(shareStringPart, wsPart, code, "A", currentRow);
                    }

                    if (location.Line)
                    {
                        AddLocationData(shareStringPart, wsPart, "Line", "B", currentRow);
                        ++currentRow;
                        AddLocationData(shareStringPart, wsPart, code, "A", currentRow);
                    }

                    // We have arr/dep if we get this far so should be easy now!
                    AddLocationData(shareStringPart, wsPart, "Arr", "B", currentRow);
                    ++currentRow;
                    AddLocationData(shareStringPart, wsPart, code, "A", currentRow);
                    AddLocationData(shareStringPart, wsPart, "Dep", "B", currentRow);
                    ++currentRow;
                }
            }
        }

        /// <summary>
        /// Check whether the cell below this timing mark has allowances
        /// </summary>
        /// <param name="trip">The current trip entry</param>
        /// <param name="nextCell">The cell contents below the current timing mark</param>
        /// <returns>True if we manage to extract allowances, otherwise false</returns>
        private bool HasAllowances(Trip trip, string nextCell)
        {
            if (nextCell == null)
                return false;

            // Get timing data
            var content = Regex.Match(nextCell, @"(\(([0-9]{1,2})\))");
            if (content.Groups.Count == 3)
            {
                trip.PathAllowance = content.Groups[2].Value.ToNullableInt();
                if (trip.PathAllowance != null)
                {
                    trip.PathAllowance = trip.PathAllowance.Value * 2;
                }
            }

            content = Regex.Match(nextCell, @"(\[([0-9]{1,2})\])");
            if (content.Groups.Count == 3)
            {
                trip.EngAllowance = content.Groups[2].Value.ToNullableInt();
                if (trip.EngAllowance != null)
                {
                    trip.EngAllowance = trip.EngAllowance.Value * 2;
                }
            }
            return trip.PathAllowance.HasValue || trip.EngAllowance.HasValue;
        }

        /// <summary>
        /// Try to extract activites from the relevant cell contents
        /// </summary>
        /// <param name="cellContents"></param>
        /// <returns></returns>
        private List<Activity> TryGetActivities(string cellContents)
        {
            
            var possibleActivities = cellContents.Split(' ').ToList();
            var activites = new List<Activity>(possibleActivities.Count);
            foreach (var act in possibleActivities)
            {
                if ( act.StartsWith("N:"))
                {
                    activites.Add(new Activity { ActivityCode = (int)Activities.Next, AssociatedTrain = act.Split(':')[1] });
                }
                else if (act.StartsWith("J:"))
                {
                    activites.Add(new Activity { ActivityCode = (int)Activities.Join, AssociatedTrain = act.Split(':')[1] });
                }
                else if (act.StartsWith("DNR:"))
                {
                    activites.Add(new Activity { ActivityCode = (int)Activities.DividesNewRear, AssociatedTrain = act.Split(':')[1] });
                }
                else if (act.StartsWith("DNF:"))
                {
                    activites.Add(new Activity { ActivityCode = (int)Activities.DividesNewFront, AssociatedTrain = act.Split(':')[1] });
                }
                else if (act.StartsWith("DER:"))
                {
                    activites.Add(new Activity { ActivityCode = (int)Activities.DetachEngineRear, AssociatedTrain = act.Split(':')[1] });
                }
                else if (act.StartsWith("DEF:"))
                {
                    activites.Add(new Activity { ActivityCode = (int)Activities.DetachEngineFront, AssociatedTrain = act.Split(':')[1] });
                }
                else if (act.StartsWith("DCR:"))
                {
                    activites.Add(new Activity { ActivityCode = (int)Activities.DropCoachesRear, AssociatedTrain = act.Split(':')[1] });
                }
                else if (act.StartsWith("DCF:"))
                {
                    activites.Add(new Activity { ActivityCode = (int)Activities.DropCoachesFront, AssociatedTrain = act.Split(':')[1] });
                }
                else if ( !Regex.IsMatch(act, "^[0-9]{4}$"))
                {
                    warning($"Unknown activity code: {act}");
                }
            }
            return activites.Any() ? activites : null;
        }

        /// <summary>
        /// Reads the various train types from the sheet, if possible, and creates entries in-memory for each
        /// </summary>
        /// <remarks>This is quite simple although there are some XRefs to lookup from the dropdown lists in the table of data</remarks>
        /// <param name="doc">The workbook</param>
        /// <param name="trainsSheet">The sheet with the trains on it</param>
        /// <returns>A list of TrainCategory, which is how the trains are represented in SimSig</returns>
        private List<TrainCategory> ProcessTrainTypes(SpreadsheetDocument doc, Sheet trainsSheet)
        {
            if (trainsSheet == null)
            {
                // Train types are not mandatory so might not be a sheet in the spreadsheet. If not, each train will use custom properties
                return new List<TrainCategory>();
            }

            WorkbookPart wbPart = doc.WorkbookPart;
            Worksheet worksheet = ((WorksheetPart)wbPart.GetPartById(trainsSheet.Id)).Worksheet;

            if ( GetCellValue(wbPart, worksheet, "A1") != "ID" )
            {
                error("No ID cell found at A1 in Train Types sheet");
                return null;
            }

            // Find first row. Allow one blank row
            var row = 2;
            if(GetCellValue(wbPart, worksheet, "A2") == string.Empty)
            {
                ++row;
            }
            if (GetCellValue(wbPart, worksheet, "A3") == string.Empty)
            {
                error("Could not find a train on rows 2 or 3 of Train Types sheet");
                return null;
            }

            var tcs = new List<TrainCategory>();
            while (GetCellValue(wbPart, worksheet, $"A{row}") != null && GetCellValue(wbPart, worksheet, $"A{row}") != string.Empty)
            {
                var sheetId = GetCellValue(wbPart, worksheet, $"A{row}");
                try
                {
                    var tc = new TrainCategory
                    {
                        SheetId = sheetId,
                        ID = trainId.IsMatch(sheetId) ? sheetId : Encryption.GenerateId(8),     // Use an id consistent with simsig if one was not provided in the spreadsheet
                        Description = GetCellValue(wbPart, worksheet, $"B{row}"),
                        MaxSpeed = Convert.ToInt32(GetCellValue(wbPart, worksheet, $"C{row}")),
                        IsFreight = Convert.ToBoolean(GetCellValue(wbPart, worksheet, $"D{row}")),
                        CanUseGoodsLines = Convert.ToBoolean(GetCellValue(wbPart, worksheet, $"E{row}")),
                        TrainLength = Convert.ToInt32(GetCellValue(wbPart, worksheet, $"F{row}")),
                        Electrification = TrainCategory.ParseElectrificationString(GetCellValue(wbPart, worksheet, $"G{row}")),
                        SpeedClass = TrainCategory.ParseSpeedClass(GetCellValue(wbPart, worksheet, $"H{row}")),
                        AccelBrakeIndex = TrainCategory.ParseAccelBrakeIndex(GetCellValue(wbPart, worksheet, $"I{row}")),
                        PowerToWeightCategory = TrainCategory.ParsePowerToWeight(GetCellValue(wbPart, worksheet, $"J{row}"))
                    };
                    if ( tcs.Any(t => t.SheetId == tc.SheetId))
                    {
                        warning($"Duplicate train {tc.SheetId} found. Linking might not work as expected");
                    }
                    tcs.Add(tc);
                    ++row;
                }
                catch(Exception)
                {
                    error("Unable to parse the data on row {row} of the Train Types sheet");
                    return null;
                }
            }

            return tcs;
        }

        /// <summary>
        /// Reads any seed groups from the sheet and returns them as a list of seed groups
        /// </summary>
        /// <param name="doc">The input workbook</param>
        /// <param name="sgSheet">The seed groups sheet or null if not present</param>
        /// <returns>Zero or more seed groups</returns>
        private List<SeedGroup> ProcessSeedGroups(SpreadsheetDocument doc, Sheet sgSheet)
        {
            if ( sgSheet == null )
            {
                // Seed groups are not mandatory so might not be a sheet in the spreadsheet
                return new List<SeedGroup>();
            }

            WorkbookPart wbPart = doc.WorkbookPart;
            Worksheet worksheet = ((WorksheetPart)wbPart.GetPartById(sgSheet.Id)).Worksheet;

            var cellValue = GetCellValue(wbPart, worksheet, "A1");
            
            return new List<SeedGroup>();
        }

        /// <summary>
        /// Helper method to get the cell's value by its alphanumeric location e.g. A1
        /// </summary>
        /// <remarks>This method is needed because of the way that Excel stores things differently depending on content</remarks>
        /// <param name="wbPart">The workbook data</param>
        /// <param name="ws">The worksheet to search</param>
        /// <param name="addressName">The alphanumeric address e.g. A1</param>
        /// <returns>The cell contents or null if the cell does not exist</returns>
        public static string GetCellValue(WorkbookPart wbPart,
            Worksheet ws,
            string addressName)
        {
            string value = null;

            // Use its Worksheet property to get a reference to the cell 
            // whose address matches the address you supplied.
            Cell theCell = ws.Descendants<Cell>().Where(c => c.CellReference == addressName).FirstOrDefault();

            if ( theCell == null )
            {
                return null;
            }

            // If the cell does not exist, return an empty string.
            if (theCell.InnerText.Length > 0)
            {
                value = theCell.InnerText;

                // If the cell represents an integer number, you are done. 
                // For dates, this code returns the serialized value that 
                // represents the date. The code handles strings and 
                // Booleans individually. For shared strings, the code 
                // looks up the corresponding value in the shared string 
                // table. For Booleans, the code converts the value into 
                // the words TRUE or FALSE.
                if (theCell.DataType != null)
                {
                    switch (theCell.DataType.Value)
                    {
                        case CellValues.SharedString:

                            // For shared strings, look up the value in the
                            // shared strings table.
                            var stringTable =
                                wbPart.GetPartsOfType<SharedStringTablePart>()
                                .FirstOrDefault();

                            // If the shared string table is missing, something 
                            // is wrong. Return the index that is in
                            // the cell. Otherwise, look up the correct text in 
                            // the table.
                            if (stringTable != null)
                            {
                                value =
                                    stringTable.SharedStringTable
                                    .ElementAt(int.Parse(value)).InnerText;
                            }
                            break;

                        case CellValues.Boolean:
                            switch (value)
                            {
                                case "0":
                                    value = "false";
                                    break;
                                default:
                                    value = "true";
                                    break;
                            }
                            break;
                    }
                }
            }
            return value;
        }

        public static Cell GetCell(Worksheet ws,string addressName)
        {
            return ws.Descendants<Cell>().Where(c => c.CellReference == addressName).FirstOrDefault();
        }

        // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
        // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }
                i++;
            }

            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
        // If the cell already exists, returns it. 
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }

        private static void AddLocationData(SharedStringTablePart shareStringPart, WorksheetPart wsPart, string contents, string column, uint row)
        {
            int ssIndex = InsertSharedStringItem(contents, shareStringPart);
            Cell cell = InsertCellInWorksheet(column, row, wsPart);
            cell.CellValue = new CellValue(ssIndex.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
        }
    }
}
