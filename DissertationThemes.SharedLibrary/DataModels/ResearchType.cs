using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DissertationThemes.SharedLibrary.DataModels
{
    /// <summary>
    /// Research type enum
    /// </summary>
    public enum ResearchType
    {
        BasicResearch = 0,
        AppliedResearch = 1,
        AppliedResearchExpDevelopment = 2
    }

    /// <summary>
    /// Handler to work with enum ResearchType
    /// </summary>
    public static class ResearchTypeHandler
    {
        public static string GetStringFromResearchType(ResearchType researchType)
        {
            switch (researchType)
            {
                case ResearchType.BasicResearch:
                    return "základný výskum";
                case ResearchType.AppliedResearch:
                    return "aplikovaný výskum";
                case ResearchType.AppliedResearchExpDevelopment:
                    return "aplikovaný výskum a experimentálny vývoj";
                default:
                    throw new ArgumentException($"Invalid ResearchType: {researchType}");
            }
        }
        public static ResearchType GetResearchTypeFromStr(string str)
        {
            switch (str)
            {
                case "základný výskum":
                    return ResearchType.BasicResearch;
                case "aplikovaný výskum a experimentálny vývoj":
                    return ResearchType.AppliedResearchExpDevelopment;
                case "aplikovaný výskum":
                    return ResearchType.AppliedResearch;
                default:
                    throw new ArgumentException($"Invalid string representation of ResearchType: {str}");
            }
        }
    }

}
