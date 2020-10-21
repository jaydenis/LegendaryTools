using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSetBuilder
{
    public class UserSettings : AppSettings<UserSettings>
    {
        public string lastFolder = @"";
        public int myInteger = 1;
    }
}
