using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavcCompiler.help
{
    public class Enums
    {
        public enum ScopeTypeEnum
        {
            Private,
            Public,
            Local,
        }
        public enum TypeEnum
        {
            String, 
            Number,
            Class,
            Void,
        }
        public enum TokenTypeEnum
        {
            String,
            Number,
            Variable,
            Return,
            Break,
            Continue,
            Null,
            New,
            Call,
            Operator,
        }
    }
}
