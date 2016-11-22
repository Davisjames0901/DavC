using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DavcCompiler.help.Enums;

namespace DavcCompiler.Types
{
    public class Data
    {
        public Data(string val, TypeEnum type)
        {
            Value = val;
            Type = Type;
        }
        public string Value { get; set; }
        public TypeEnum Type { get; set; }
    }
}
