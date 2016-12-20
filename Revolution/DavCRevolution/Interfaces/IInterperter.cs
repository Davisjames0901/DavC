using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavCRevolution.Interfaces.ICodeObjects;
using DavCRevolution.Interfaces.ICodeObjects.ILine;

namespace DavCRevolution.Interfaces
{
    public interface IInterperter
    {
        string InterpretLine(ISourceFile context, ILine line);
    }
}
