using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DissertationThemes.SharedLibrary.DataModels
{
    public class Supervisor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Theme> Themes { get; set; } = new List<Theme>();

    }
}
