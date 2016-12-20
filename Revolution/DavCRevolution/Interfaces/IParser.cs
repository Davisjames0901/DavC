using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavCRevolution.Interfaces.ICodeObjects;

namespace DavCRevolution.Interfaces
{
    public interface IParser
    {
        ISourceFile ParseSourceFile(string file);

        string FormatString(string s);
    }
}
