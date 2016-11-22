using DavcCompiler.help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DavcCompiler.help.Enums;

namespace DavcCompiler.Types
{
    public class Parameters
    {
        public Parameters(string s)
        {
            var tokens = s.Split(' ');
            Type = Helpers.EvaluateTypeToken(tokens[0]);
            Name = tokens[1];
        }
        public TypeEnum Type { get; set; }
        public string Name { get; set; }
        
    }
}
