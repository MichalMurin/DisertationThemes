using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DissertationThemes.SharedLibrary.DataModels
{
    public class Theme
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsExternalStudy { get; set; }
        public bool IsFullTimeStudy { get; set; }
        public ResearchType ResearchType { get; set; }
        public StProgram StProgram { get; set; } = new StProgram();
        public int StProgramId { get; set; }
        public Supervisor Supervisor { get; set; } = new Supervisor();
        public int SupervisorId { get; set; }
    }
}
