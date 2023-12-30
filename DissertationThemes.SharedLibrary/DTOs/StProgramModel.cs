using DissertationThemes.SharedLibrary.DataModels;
using System.Text.Json.Serialization;

namespace DissertationThemes.SharedLibrary.DTOs
{
    /// <summary>
    /// Model of Strudy program to represent it in Json responses
    /// </summary>
    public class StProgramModel
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("fieldOfStudy")]
        public string FieldOfStudy { get; set; } = string.Empty;

        [JsonConstructor]
        public StProgramModel()
        {
            
        }
        public StProgramModel(StProgram program)
        {
            Id = program.Id;
            Name = program.Name;
            FieldOfStudy = program.FieldOfStudy;
        }
    }
}
