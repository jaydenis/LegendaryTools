using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryData
{
    public class LegendarySettings : AppSettings<LegendarySettings>
    {
        public bool HAS_BEEN_UPDATED = false;
        public DateTime? LAST_UPDATED { get; set; }

    }
}
