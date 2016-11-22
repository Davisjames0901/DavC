using DavcCompiler.help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DavcCompiler.help.Enums;

namespace DavcCompiler.Types
{
    public sealed class SourceFile : CodeObject
    {
        private static string[] _illeagleSymboles = { };
        public SourceFile(string fileName, List<string> source)
        {
            Methods = new List<Method>();
            Classes = new List<Class>();
            GlobalVariables = new List<Variable>();
            Imports = new List<SourceFile>();
            FileName = fileName;
            Name = ParseName(fileName);
            ProcessSource(source);
        }
        public string FileName { get; set; }
        public static List<Method> Methods { get; set; }
        public List<Class> Classes { get; set; }
        public static Method MainMethod { get; set; }
        public static List<Variable> GlobalVariables { get; set; }
        public static List<SourceFile> Imports { get; set; }

        private void AddMethod(string s, ScopeTypeEnum access)
        {
            var method = new Method(s, this, access);
            if (method.Name != "main")
            {
                if (Methods.Any(x => x.Name == method.Name))
                {
                    throw new Exception(string.Format("Cannot Create {0} Global Method already exists.", method.Name));
                }
                Methods.Add(method);
            }
            else if (MainMethod != null)
            {
                throw new Exception("Ambiguos Main method found.");
            }
            else
            {
                MainMethod = method;
            }
        }
        private void AddVariable(ScopeTypeEnum access, string s)
        {
            var var = new Variable(s, this, access);
            if (HasVariable(var.Name))
            {
                throw new Exception(string.Format("Cannot Create {0} Global Variable already exists.", var.Name));
            }
            GlobalVariables.Add(var);
        }
        private void AddClass(ScopeTypeEnum access, string s)
        {
            var cls = new Class(access, s, this);
            if (HasClass(cls.Name))
            {
                throw new Exception(string.Format("Cannot Create Class {0} class already exists.", cls.Name));
            }
            Classes.Add(cls);
        }
        private void AddImport(string s)
        {
            var fileName = string.Empty;
            if (Imports.Any(x => x.FileName == fileName))
            {
                Console.WriteLine("Duplicate Import " + fileName);
            }
            //TODO imports
        }

        public bool HasClass(string s)
        {
            return Classes.Any(x => x.Name == s);
        }
        public Data Start()
        {
            var call = new MethodCall("main", null);
            return MainMethod.ExecuteMethod(call);
        }
        #region ParseMethods
        private string ParseName(string s)
        {
            var tokens1 = s.Split('\\');
            var tokens2 = tokens1[tokens1.Length - 1].Split('.');
            return tokens2[0];
        }
        private void ProcessSource(List<string> source)
        {
            ScopeTypeEnum? identifier = null;
            var start = false;
            foreach (var line in source)
            {
                if (!start)
                {
                    if (line.StartsWith("import"))
                    {
                        AddImport(line);
                    }
                    else if (line.EndsWith(":"))
                    {
                        identifier = Helpers.ParseIdentifier(line);
                        start = true;
                    }
                }
                else
                {
                    if (line.StartsWith("class"))
                    {
                        this.CheckIdentifier(identifier, line);
                        AddClass(identifier.Value, line);
                    }
                    else if (line.StartsWith("def"))
                    {
                        this.CheckIdentifier(identifier, line);
                        AddMethod(line, identifier.Value);
                    }
                    else if (line.EndsWith(":"))
                    {
                        identifier = Helpers.ParseIdentifier(line);
                        start = true;
                    }
                    else
                    {
                        this.CheckIdentifier(identifier, line);
                        AddVariable(identifier.Value, line);
                    }
                }
            }
        }
        #endregion
        #region CodeObject
        public override Variable GetVariable(string var)
        {
            if (HasVariable(var))
            {
                return GlobalVariables.Single(x => x.Name == var);
            }
            ThrowError(string.Format("Variable {0} doesnt exist in current context.", var));
            return null;
        }

        public override void SetVariable(string var, string val)
        {
            if (HasVariable(var))
            {
                var v = GlobalVariables.Single(x => x.Name == var);
                var type = Helpers.GetType(val);
                if (v.Type != type)
                {
                    ThrowError(string.Format("Variable {0} of type {1} cannot be assigned to {2}", v.Name, v.Type, type));
                }
            }
            ThrowError(string.Format("Variable {0} doesnt exist in current context.", var));
        }

        public override bool HasVariable(string var)
        {
            return GlobalVariables.Any(x => x.Name == var);
        }

        public override Data MakeMethodCall(MethodCall call)
        {
            if (Methods.Any(x => x.Name == call.MethodName))
            {
                foreach (var item in Methods.Where(x => x.Name == call.MethodName))
                {
                    var i = 0;
                    if (item.Parameters.Count() == call.Parameters.Count)
                    {
                        var match = true;
                        foreach (var para in item.Parameters)
                        {
                            i++;
                            if (para.Type != call.Parameters[i].Type)
                            {
                                match = false;
                            }
                        }
                        if (match)
                        {
                            return item.ExecuteMethod(call);
                        }
                    }
                }
            }
            return Parent.MakeMethodCall(call);
        }
        #endregion
    } 
}
