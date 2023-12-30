using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DissertationThemes.SharedLibrary.DataModels;

namespace DissertationThemes.SharedLibrary.DTOs
{
    /// <summary>
    /// Model for theme to represent it in Json responses
    /// </summary>
    public class ThemeModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("supervisor")]
        public string Supervisor { get; set; } = string.Empty;
        [JsonPropertyName("stProgramId")]
        public int StProgramId { get; set; }
        [JsonPropertyName("isFullTimeStudy")]
        public bool IsFullTimeStudy { get; set; }
        [JsonPropertyName("isExternalStudy")]
        public bool IsExternalStudy { get; set; }
        [JsonPropertyName("researchType")]
        public ResearchType ResearchType { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonConstructor]
        public ThemeModel()
        {
        }

        public ThemeModel(Theme theme)
        {
            Id = theme.Id;
            Name = theme.Name;
            Supervisor = theme.Supervisor.Name;
            StProgramId = theme.StProgramId;
            IsFullTimeStudy = theme.IsFullTimeStudy;
            IsExternalStudy = theme.IsExternalStudy;
            ResearchType = theme.ResearchType;
            Description = theme.Description;
            Created = theme.Created;
        }
    }
}
