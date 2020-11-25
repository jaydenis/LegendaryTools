using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryData.Models
{
    public class BaseCardViewModel
    {
        public String Name { get; set; }

        public String BGGLink { get; set; }

        public String Author { get; set; }

        public String ExpansionName { get; set; }

        public String UniverseName { get; set; }

        public String Year { get; set; }

        public String TeamName { get; set; }

        public Boolean? IsOfficial { get; set; }
    }

    [NotMapped]
    public class AuthorViewModel  
    {
        public String Name { get; set; }
        public String BGGLink { get; set; }
        public IEnumerable<BaseCardViewModel> HeroesList { get; set; }
        public IEnumerable<BaseCardViewModel> MastermindList { get; set; }
        public IEnumerable<BaseCardViewModel> HenchmenList { get; set; }
        public IEnumerable<BaseCardViewModel> VillainGroupList { get; set; }
        public IEnumerable<TeamsViewModel> TeamsList { get; set; }
    }

    [NotMapped]
    public class TeamsViewModel
    {
        public String Name { get; set; }

        public String BGGLink { get; set; }
        public string UniverseName { get; set; }
        public IEnumerable<Heroes> HeroesList { get; set; }
    }

    
}
