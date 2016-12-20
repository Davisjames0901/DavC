using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavCRevolution.Interfaces.ICodeObjects.ILine;

namespace DavCRevolution.CodeObjects.Standard
{
    public class ComputeLineWithMethod : ComputeLine, IComputeLineWithMethod
    {
        public string[] Args { get; set; }
        public string[] Name { get; set; }
    }
}
