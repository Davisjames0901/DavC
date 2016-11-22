using DavcCompiler.help;
using DavcCompiler.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DavcCompiler.lib
{
    public static class Methods
    {
        public static Data MakeMethodCall(MethodCall call)
        {
            switch (call.MethodName)
            {
                case "print":
                    print(call.Parameters);
                    break;
                default:
                    break;
            }
            return null;
        }
        public static void print(List<Data> data)
        {
            var s = string.Empty;
            foreach(var item in data)
            {
                s += item.Value;
            }
            Console.WriteLine(s);
        }
        
        public static string Concat(params string[] values)
        {
            if(values.Any(x=>x.isString()))
            {
                return string.Concat(values);
            }
            return null;
        }
    }
}
