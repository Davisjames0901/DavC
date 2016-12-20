using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavCRevolution.Interfaces;
using DavCRevolution.Interfaces.ICodeObjects.ILine;

namespace DavCRevolution.CodeObjects.Standard
{
    public class MethodCall : IMethodCall
    {
        public string[] Args { get; set; }

        public string[] Name { get; set; }

        public ICodeObject Parent { get; set; }

        public string Text { get; set; }

        public string GetTrace()
        {
            throw new NotImplementedException();
        }
    }
}
