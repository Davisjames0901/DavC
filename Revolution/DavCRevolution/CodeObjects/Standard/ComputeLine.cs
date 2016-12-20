using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavCRevolution.Interfaces;
using DavCRevolution.Interfaces.ICodeObjects.ILine;

namespace DavCRevolution.CodeObjects.Standard
{
    public class ComputeLine : IComputeLine
    {
        public ICodeObject Parent { get; set; }
        public string Text { get; set; }

        public string GetTrace()
        {
            throw new NotImplementedException();
        }
    }
}
