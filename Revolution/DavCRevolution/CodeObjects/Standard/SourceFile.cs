using System;
using System.Collections.Generic;
using DavCRevolution.Interfaces;
using DavCRevolution.Interfaces.ICodeObjects;
using DavCRevolution.Interfaces.ICodeObjects.ILine;

namespace DavCRevolution.CodeObjects.Standard
{
    public class SourceFile : ISourceFile
    {
        public ISourceFile Imports { get; set; }

        public string Name { get; set; }

        public void AddClass(IClass @class)
        {
            throw new NotImplementedException();
        }

        public void AddMethod(IMethod method)
        {
            throw new NotImplementedException();
        }

        public void AddVariable(IVariable variable)
        {
            throw new NotImplementedException();
        }

        public string CallMethod(IMethodCall method)
        {
            throw new NotImplementedException();
        }

        public IClass GetClass(string name, params string[] args)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IClass> GetClasses()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMethod> GetMethods()
        {
            throw new NotImplementedException();
        }

        public IVariable GetVariable(string variableName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVariable> GetVariables()
        {
            throw new NotImplementedException();
        }

        public void SetVariable(string variableName, string variableValue)
        {
            throw new NotImplementedException();
        }

        public void StartClass(IClass @class)
        {
            throw new NotImplementedException();
        }
    }
}
