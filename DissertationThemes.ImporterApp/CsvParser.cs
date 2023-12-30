using DissertationThemes.SharedLibrary;
using DissertationThemes.SharedLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DissertationThemes.ImporterApp
{
    public static class CsvParser
    {
        /// <summary>
        /// Parse csv file to Database
        /// </summary>
        /// <param name="filename">path to csv file</param>
        /// <param name="clearDb">true - Db will be cleared before inserting new values</param>
        /// <returns></returns>
        public static bool ParseFileAndSaveToDb(string filename, bool clearDb = false)
        {
            if (clearDb) 
            { 
                DatabaseService.ClearDatabase();
            }

            var lines = File.ReadAllLines(filename);

            string themeName, supervisorName, StProgramName, fieldOfStudy, description;
            bool isFullTimeStudy, isExternalStudy;
            ResearchType researchType;
            DateTime created;
            
            foreach (var line in lines.Skip(1))
            {
                var data = line.Split(";");
                themeName = data[0];
                supervisorName = data[1];
                StProgramName = data[2];
                fieldOfStudy = data[3];
                bool.TryParse(data[4], out isFullTimeStudy);
                bool.TryParse(data[5], out isExternalStudy);
                researchType = ResearchTypeHandler.GetResearchTypeFromStr(data[6]);
                description = data[7].Replace("<br>", Environment.NewLine);
                DateTime.TryParseExact(data[8], "d.M.yyyy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out created);

                var importResult = DatabaseService.TryInsertThemeData(themeName, supervisorName,
                                                                      StProgramName, fieldOfStudy,
                                                                      isFullTimeStudy, isExternalStudy,
                                                                      researchType, description, created);
                if (!importResult)
                {
                    Console.WriteLine($"Theme with the name: {themeName} was not imported succefuly!");
                }
            }
            return true;
        }
    }
}
