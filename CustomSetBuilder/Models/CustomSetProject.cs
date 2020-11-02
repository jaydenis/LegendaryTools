using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSetBuilder.Models
{
    public class CustomSetProject
    {
        public string CustomSetProjectName { get; set; }
        public string ProjectLoaction { get; set; }
        public int CardSpacing { get; set; }
        public List<CustomCard> CardList { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    public class CustomCard
    {
        public string CardPath { get; set; }
    }
}
