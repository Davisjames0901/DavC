using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavCRevolution.Interfaces
{
    public interface ICodeObject
    {
        ICodeObject Parent { get; set; }
        string GetTrace();
    }
}
