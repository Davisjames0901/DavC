using DavcCompiler.help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DavcCompiler.help.Enums;

namespace DavcCompiler.Types
{
    public class Variable : CodeObject
    {
        private static string[] _illeagleSymboles = { };
        public Variable(string s, CodeObject parent, ScopeTypeEnum scope)
        {
            Parent = parent;
            AccessLevel = scope;
            //do parse work
        }
        public Variable(string var, string val, CodeObject parent, ScopeTypeEnum scope)
        {
            Parent = parent;
            AccessLevel = scope;
            Name = var;
            Value = val;
            Type = Helpers.GetType(val);
            //do parse work
        }
        public string Value { get; set; }
        public TypeEnum Type { get; set; }
        public ScopeTypeEnum AccessLevel { get; set; }

        #region CodeObject

        public override Variable GetVariable(string var)
        {
            return Parent.GetVariable(var);
        }

        public override void SetVariable(string var, string val)
        {
            Parent.SetVariable(var,val);
        }

        public override bool HasVariable(string var)
        {
            return false;
        }

        public override Data MakeMethodCall(MethodCall call)
        {
            return Parent.MakeMethodCall(call);
        }
        #endregion
    }
}
