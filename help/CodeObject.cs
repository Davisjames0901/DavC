using DavcCompiler.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavcCompiler.help
{
    public abstract class CodeObject
    {
        string[] illeagleCharicters;
        public string Name { get; set; }
        public string GetTrace()
        {
            var trace = Name;
            var currentParent = Parent;
            while(currentParent!=null)
            {
                trace = currentParent.Name + "." + trace;
                currentParent = currentParent.Parent; 
            }
            return trace;
        }
        public void ThrowError(string message)
        {
            throw new Exception(message+ "\n Trace:" + GetTrace());
        }
        public abstract Variable GetVariable(string var);
        public abstract void SetVariable(string var, string val);
        public CodeObject Parent { get; set; }
        public abstract bool HasVariable(string var);
        public abstract Data MakeMethodCall(MethodCall call);
    }
}
