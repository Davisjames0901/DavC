using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavCRevolution.Interfaces.ICodeObjects.ILine;

namespace DavCRevolution.Interfaces
{
    public interface IMethod : ICodeObject
    {
        string Name{ get; set; }
        string[] Args { get; set; }
        List<ILine> Lines { get; set; }
    }
}
