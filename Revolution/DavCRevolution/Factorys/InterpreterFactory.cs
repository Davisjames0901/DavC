using DavCRevolution.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavCRevolution.lib;

namespace DavCRevolution.Factorys
{
    public static class InterpreterFactory
    {
        public static IInterperter CreateInterperter()
        {
            return new Interpreter();
        }
    }
}
