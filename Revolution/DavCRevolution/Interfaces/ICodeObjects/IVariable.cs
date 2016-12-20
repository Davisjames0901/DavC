using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavCRevolution.Interfaces.ICodeObjects
{
    public interface IVariable : ICodeObject
    {
        string Name { get; set; }
        string Value { get; set; }
    }
}
