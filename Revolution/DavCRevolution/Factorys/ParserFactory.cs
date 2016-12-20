using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavCRevolution.Interfaces;
using DavCRevolution.lib;

namespace DavCRevolution.Factorys
{
    public static class ParserFactory
    {
        public static IParser CreateParser()
        {
            return new Parser();
        }
    }
}
