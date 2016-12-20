using System.Collections.Generic;
using DavCRevolution.Interfaces.ICodeObjects.ILine;

namespace DavCRevolution.Interfaces.ICodeObjects
{
    public interface ISourceFile
    {
        string Name { get; set; }
        ISourceFile Imports { get; set; }

        #region variable
        IEnumerable<IVariable> GetVariables();
        void AddVariable(IVariable variable);
        void SetVariable(string variableName, string variableValue);
        IVariable GetVariable(string variableName);
        #endregion
        #region method

        IEnumerable<IMethod> GetMethods();
        void AddMethod(IMethod method);
        string CallMethod(IMethodCall method);

        #endregion
        #region Class

        IEnumerable<IClass> GetClasses();
        void AddClass(IClass @class);
        IClass GetClass(string name, params string[] args);
        void StartClass(IClass @class);

        #endregion
    }
}
