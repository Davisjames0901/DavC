using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavcCompiler.Types
{
    public class MethodCall
    {
        public MethodCall(string methodName, params Data[] parameters)
        {
            MethodName = methodName;
            Parameters = parameters == null?new List<Data>():parameters.ToList();
        }
        public string MethodName { get; set; }
        public List<Data> Parameters { get; set; }
    }
}
