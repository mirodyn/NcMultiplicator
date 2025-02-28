using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NcMultiplicator
{
    internal class Options
    {
        [Option('a', "axis", Required = false, HelpText = "Osa, která má být násobena")]
        public string Axis { get; set; }

        [Option('f', "file", Required = false, HelpText = "Zdrojový soubor")]
        public string File { get; set; }
      
        [Option('c', "coefficient", Required = false, HelpText = "Koeficient pro násobení")]
        public string Coefficient { get; set; }

        public Options()
        {
            Axis = string.Empty;
            File = string.Empty;
            Coefficient = string.Empty;
        }
    }
}
