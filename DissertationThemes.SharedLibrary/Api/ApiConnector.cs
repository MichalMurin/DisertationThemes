using DissertationThemes.SharedLibrary;
using DissertationThemes.SharedLibrary.DTOs;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text.Json;
using System.Threading.Tasks;

namespace DissertationThemes.SharedLibrary.Api
{
    public class ApiConnector
    {
        /// <summary>
        /// Http client for api calls
        /// </summary>
        public HttpClient Client { get; set; } = new HttpClient();
        /// <summary>
        /// Request url for api calls
        /// </summary>
        public string RequestUrl { get; } = CommonDeffinitions.ApiUrlPrefix;

        public ApiConnector()
        {            
        }
        /// <summary>
        /// Get json result from url
        /// </summary>
        /// <param name="url">api url request</param>
        /// <returns>json string</returns>
        private async Task<string> GetJsonResult(string url)
        {
            // Send the GET request
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = Client.Send(request);

            // Check response status code
            if (response.IsSuccessStatusCode)
            {
                // Parse JSON response
                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return string.Empty;
            }
        }
        /// <summary>
        /// Get byte array from api request
        /// </summary>
        /// <param name="url">url request</param>
        /// <returns>byte array from api response</returns>
        private async Task<byte[]?> GetByteResult(string url)
        {
            // Send the GET request
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = Client.Send(request);

            // Check response status code
            if (response.IsSuccessStatusCode)
            {
                // Parse JSON response
                var content = await response.Content.ReadAsByteArrayAsync();
                return content;
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return null;
            }
        }
        /// <summary>
        /// Get themes from API
        /// </summary>
        /// <param name="year">create year of Theme</param>
        /// <param name="program">study program id</param>
        /// <returns>list of themes</returns>
        public async Task<List<ThemeModel>?> GetThemes(int year = -1, int program = -1)
        {
            var json = await GetJsonResult($"{RequestUrl}themes?year={year}&stProgramId={program}");
            var themes = JsonSerializer.Deserialize<List<ThemeModel>>(json);
            return themes;
        }
        /// <summary>
        /// Get study programs
        /// </summary>
        /// <returns>List of study programs</returns>
        public async Task<List<StProgramModel>?> GetPrograms()
        {
            var json = await GetJsonResult($"{RequestUrl}stprograms");
            var programs = JsonSerializer.Deserialize<List<StProgramModel>>(json);
            if (programs is not null)
            {
                return programs;
            }
            return null;
        }
        /// <summary>
        /// Get years
        /// </summary>
        /// <returns>Get list of all years from themes</returns>
        public async Task<List<int>?> GetYears()
        {
            var json = await GetJsonResult($"{RequestUrl}themesyears");
            var years = JsonSerializer.Deserialize<List<int>>(json);
            return years;
        }
        /// <summary>
        /// Get byte representation of docx file
        /// </summary>
        /// <param name="id">Theme ID</param>
        /// <returns>Byte represetnation of docx file</returns>
        public async Task<byte[]?> GetDocx(int id)
        {
            try
            {
                return await GetByteResult($"{RequestUrl}theme2docx/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// Get csv file
        /// </summary>
        /// <param name="year">Theme created year</param>
        /// <param name="programId">Study program id</param>
        /// <returns>byte representation of csv file</returns>
        public async Task<byte[]?> GetCsv(int year = -1, int programId = -1)
        {
            try
            {
                return await GetByteResult($"{RequestUrl}themes2csv?year={year}&stProgramId={programId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
