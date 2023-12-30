using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Packaging;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using DissertationThemes.SharedLibrary.DataModels;
using DissertationThemes.SharedLibrary.DataBase;
namespace DissertationThemes.SharedLibrary
{
    /// <summary>
    /// Service to work with DB
    /// </summary>
    public static class DatabaseService
    {
        /// <summary>
        /// Get Themem from Db based on ID
        /// </summary>
        /// <param name="id">Themem ID</param>
        /// <returns>Theme</returns>
        public static Theme? GetThemeByiD(int id)
        {
            using (var context = new ThemesDbContext())
            {
                var result = context.Theme.FirstOrDefault(theme => theme.Id == id);
                if (result is not null)
                {
                    FillThemesObjectValues(result);
                }
                return result;
            }
        }
        /// <summary>
        /// Get Study programs
        /// </summary>
        /// <returns>List of study programs</returns>
        public static List<StProgram> GetStProgramsModels()
        {
            using (var context = new ThemesDbContext())
            {
                var result = context.StProgram;
                return result.OrderBy(program => program.Name).ToList();
            }
        }
        /// <summary>
        /// Insetrts themem data to DB
        /// </summary>
        /// <param name="themeName"></param>
        /// <param name="supervisorName"></param>
        /// <param name="stProgramName"></param>
        /// <param name="fieldOfStudy"></param>
        /// <param name="isFullTimeStudy"></param>
        /// <param name="isExternalStudy"></param>
        /// <param name="researchType"></param>
        /// <param name="description"></param>
        /// <param name="created"></param>
        /// <returns></returns>
        public static bool TryInsertThemeData(string themeName, string supervisorName,
                                      string stProgramName, string fieldOfStudy, 
                                      bool isFullTimeStudy, bool isExternalStudy,
                                      ResearchType researchType, string description,
                                      DateTime created)
        {
            using (var context = new ThemesDbContext())
            {

                var theme = context.Theme.FirstOrDefault(theme => theme.Name == themeName &&
                                                         theme.Description == description && 
                                                         theme.StProgram.Name == stProgramName && 
                                                         theme.Created == created);
                if (theme != null)
                {
                    // theme with the same already exists, returning false
                    return false;
                }
                theme = new Theme
                {
                    Name = themeName,
                    Created = created,
                    Description = description,
                    IsExternalStudy = isExternalStudy,
                    IsFullTimeStudy = isFullTimeStudy,
                    ResearchType = researchType
                };

                var supervisor = context.Supervisor.FirstOrDefault(supervisor => supervisor.Name == supervisorName);
                if (supervisor == null)
                {
                    supervisor = new Supervisor
                    {
                        Name = supervisorName
                    };
                    context.Supervisor.Add(supervisor);
                }
                supervisor.Themes.Add(theme);
                theme.Supervisor = supervisor;

                var stProgram = context.StProgram.FirstOrDefault(program => program.Name == stProgramName);
                if (stProgram == null)
                {
                    stProgram = new StProgram
                    {
                        Name = stProgramName,
                        FieldOfStudy = fieldOfStudy
                    };
                    context.StProgram.Add(stProgram);
                }
                stProgram.Themes.Add(theme);
                theme.StProgram = stProgram;

                context.Theme.Add(theme);
                context.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// Clear database
        /// </summary>
        public static void ClearDatabase()
        {
            using (var context = new ThemesDbContext())
            {
                context.Theme.ExecuteDelete();
                context.Supervisor.ExecuteDelete();
                context.StProgram.ExecuteDelete();
                context.SaveChanges(true);
            }
        }
        /// <summary>
        /// Get path to generated docx file for theme
        /// </summary>
        /// <param name="id">Theme ID</param>
        /// <returns>path to generated docx file</returns>
        public static string? GetThemeDocxPath(int id)
        {
            var theme = GetThemeByiD(id);
            if (theme is not null)
            {
                // Path to the Word document template
                var templatePath = "..\\Resources\\PhD_temy_sablona.docx";

                // Values to replace in the template
                var replaceValues = new Dictionary<string, string>
                {
                    {"#=ThemeName=#", theme.Name },
                    {"#=Supervisor=#", theme.Supervisor.Name },
                    {"#=StProgram=#", theme.StProgram.Name },
                    {"#=FieldOfStudy=#", theme.StProgram.FieldOfStudy},
                    {"#=ResearchType=#", ResearchTypeHandler.GetStringFromResearchType(theme.ResearchType) },
                    {"#=Description=#", theme.Description }
                };

                // Create a copy of the template
                var outputFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_output.docx");
                File.Copy(templatePath, outputFilePath);
                // Replace placeholders in the Word document
                using (var wordDocument = WordprocessingDocument.Open(outputFilePath, true))
                {
                    if (wordDocument.MainDocumentPart is null)
                        return null;
                    var body = wordDocument.MainDocumentPart.Document.Body;
                    if (body is null) 
                        return null;
                    bool processingPlaceHolder = false;
                    string tmpString = string.Empty;
                    foreach (var textElement in body.Descendants<Text>())
                    {
                        if (textElement.Text.StartsWith("#") && !processingPlaceHolder)
                        {
                            tmpString = textElement.Text;
                            textElement.Text = "";
                            processingPlaceHolder = true;
                        }
                        else if (textElement.Text.EndsWith("#") && processingPlaceHolder)
                        {
                            tmpString += textElement.Text;
                            textElement.Text = "";
                            processingPlaceHolder = false;

                            foreach (var replaceValue in replaceValues)
                            {
                                if (tmpString.Contains(replaceValue.Key))
                                {
                                    tmpString = replaceValue.Value;
                                    textElement.Text = replaceValue.Value;
                                    break;
                                }
                            }
                        }
                        else if (processingPlaceHolder)
                        {
                            tmpString += textElement.Text;
                            textElement.Text = "";
                        }
                    }
                }
                return outputFilePath; 
            }
            return null;
        }
        /// <summary>
        /// Get Themes
        /// </summary>
        /// <param name="year">Created year of Theme</param>
        /// <param name="stProgramId">Id of Study program</param>
        /// <returns>List of filtered themes</returns>
        public static List<Theme> GetThemesByYearAndProgramId(int year, int stProgramId)
        {
            using (var context = new ThemesDbContext())
            {
                var result = context.Theme.Where(theme => (year > 0 ? theme.Created.Year == year : true) && 
                                                          (stProgramId > 0 ? theme.StProgramId == stProgramId : true));
                foreach (var theme in result)
                {
                    FillThemesObjectValues(theme);
                }
                return result.ToList();
            }
        }
        /// <summary>
        /// Get path to csv file
        /// </summary>
        /// <param name="year">created year of theme</param>
        /// <param name="stProgramId">Id of study program</param>
        /// <returns>path to generated CSV file with filtered themes</returns>
        public static string GetThemesCsvCintentByYearANdStProgramId(int year, int stProgramId)
        {
            using (var context = new ThemesDbContext())
            {
                var result = context.Theme.Where(theme => (year > 0 ? theme.Created.Year == year : true) &&
                                                          (stProgramId > 0 ? theme.StProgramId == stProgramId : true));
                var csvContent = $"Name;Supervisor;StProgram;FieldOfStudy;IsFullTimeStudy;IsExternalStudy;ResearchType;Descrition;Created{Environment.NewLine}";
                foreach (var theme in result)
                {
                    FillThemesObjectValues(theme);
                    csvContent += GetCsvLineForTheme(theme);
                }
                return csvContent;
            }
        }
        /// <summary>
        /// Get all years when themes were created
        /// </summary>
        /// <returns>List of years</returns>
        public static List<int> GetThemesYears()
        {
            using (var context = new ThemesDbContext())
            {
                var result = context.Theme.Select(theme => theme.Created.Year).Distinct().OrderByDescending(t => t).ToList();
                return result;
            }
        }
        /// <summary>
        /// Update THemes object atributes
        /// </summary>
        /// <param name="theme">theme, where the object atributes will be updated</param>
        private static void FillThemesObjectValues(Theme theme)
        {
            using (var context = new ThemesDbContext())
            {
                theme.Supervisor = context.Supervisor.FirstOrDefault(s => s.Id == theme.SupervisorId) ?? new Supervisor();
                theme.StProgram = context.StProgram.FirstOrDefault(p => p.Id == theme.StProgramId) ?? new StProgram();
            };
        }
        /// <summary>
        /// Get line for csv file for one theme
        /// </summary>
        /// <param name="theme">theme</param>
        /// <returns>line with delimited data for csv file</returns>
        private static string GetCsvLineForTheme(Theme theme)
        {
            return $"{theme.Name};{theme.Supervisor.Name};{theme.StProgram.Name};" +
                   $"{theme.StProgram.FieldOfStudy};{theme.IsFullTimeStudy};" +
                   $"{theme.IsExternalStudy};{ResearchTypeHandler.GetStringFromResearchType(theme.ResearchType)};" +
                   $"{theme.Description.Replace(Environment.NewLine, "<br>")};{theme.Created.ToString("d.M.yyyy H:mm")}{Environment.NewLine}";
        }
    }
}
