using DissertationThemes.SharedLibrary;
using DissertationThemes.SharedLibrary.DTOs;
using DocumentFormat.OpenXml.Drawing;
using System.Text;
namespace DissertationThemes.WebApi
{
    /// <summary>
    /// Facotry for creating API endpoints 
    /// </summary>
    public class EndpointsFactory
    {
        public WebApplication App { get; set; }
        public EndpointsFactory(WebApplication application) 
        {
            App = application;
        }

        public void CreateGetStProgramsEndpoint()
        {
            App.MapGet("/stprograms", () =>
            {
                var programs = DatabaseService.GetStProgramsModels();
                return programs.Select(program => new StProgramModel(program));
            });
        }

        public void CreateGetThemeEndpoint()
        {
            App.MapGet("/theme/{id}", IResult (int id) =>
            {
                var theme = DatabaseService.GetThemeByiD(id);
                if (theme is null)
                {
                    return TypedResults.NotFound();
                }
                else
                {
                    return TypedResults.Ok(new ThemeModel(theme));
                }
            });
        }

        public void CreateGetDocxThemeEndpoint()
        {
            App.MapGet("/theme2docx/{id}", IResult (int id) =>
            {
                var path = DatabaseService.GetThemeDocxPath(id);
                if (path is null)
                {
                    return TypedResults.NotFound();
                }
                else
                {
                    // Set content type for Word document
                    var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    // Return the modified Word document as a response
                    var stream = new FileStream(path, FileMode.Open);
                    return Results.File(stream, contentType, $"PhD-theme-{id}.docx");
                }
            });
        }

        public void CreateGetThemesByYearAndProgramEndpoint()
        {
            App.MapGet("/themes", (int year = -1, int stProgramId = -1) =>
            {
                var result = DatabaseService.GetThemesByYearAndProgramId(year, stProgramId);
                return TypedResults.Ok(result.Select(theme => new ThemeModel(theme)));
            });
        }

        public void CreateGetThemes2CsvEndpoint()
        {
            App.MapGet("/themes2csv", (int year = -1, int stProgramId = -1) =>
            {
                var csvContent = DatabaseService.GetThemesCsvCintentByYearANdStProgramId(year, stProgramId);
                var csvBytes = Encoding.UTF8.GetBytes(csvContent);
                // Set content type for CSV
                var contentType = "text/csv";
                // Return the CSV file as a response
                return Results.File(csvBytes, contentType, "themes.csv");
            });
        }

        public void CreateGetThemesYearsEndpoint()
        {
            App.MapGet("/themesyears", () =>
            {
                return DatabaseService.GetThemesYears();
            });
        }


    }
}
