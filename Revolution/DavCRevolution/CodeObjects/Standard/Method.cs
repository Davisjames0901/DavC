using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavCRevolution.Interfaces;
using DavCRevolution.Interfaces.ICodeObjects.ILine;

namespace DavCRevolution.CodeObjects.Standard
{
    public class Method : IMethod
    {
        public string[] Args { get; set; }

        public List<ILine> Lines { get; set; }

        public string Name { get; set; }

        public ICodeObject Parent { get; set; }

        public string GetTrace()
        {
            throw new NotImplementedException();
        }
    }
}
