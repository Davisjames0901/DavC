using DavcCompiler.help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DavcCompiler.help.Enums;

namespace DavcCompiler.Types
{
    public class Class : CodeObject
    {
        private static string[] _illeagleSymboles = { };
        public Class(ScopeTypeEnum access, string s, CodeObject parent)
        {
            Methods = new List<Method>();
            Variables = new List<Variable>();
            Lines = new List<Line>();
            Parent = parent;
            AccessLevel = access;
            Name = ParseName(s);
            ParseCode(s);
        }
        public Method Constructor { get; set; }
        public List<Method> Methods { get; set; }
        public List<Variable> Variables { get; set; }
        public List<Line> Lines { get; set; }
        public ScopeTypeEnum AccessLevel { get; set; }

        private void AddMethod(string s, ScopeTypeEnum access)
        {
            var method = new Method(s,this, access);
                if (Methods.Any(x => x.Name == method.Name))
                {
                    throw new Exception(string.Format("Cannot Create {0} Method already exists.", method.Name));
                }
                Methods.Add(method);
        }
        private void AddVariable(string s, ScopeTypeEnum access)
        {
            var var = new Variable(s,this,access);
            if (HasVariable(var.Name)||Parent.HasVariable(var.Name))
            {
                throw new Exception(string.Format("Cannot Create {0} Variable already exists.", var.Name));
            }
            Variables.Add(new Variable(s, this, access));
        }

        #region ParseMethods
        private string ParseName(string s)
        {
            return s.Substring(5, s.IndexOf('{') -5).Trim();
        }
        private void ParseCode(string s)
        {
            var code = s.GetTextBetween('{', '}');
            var lines = code.Beautify();
            ScopeTypeEnum? scope = null;
            foreach(var line in lines)
            {
                if(line.EndsWith(":"))
                {
                    scope = Helpers.ParseIdentifier(line);
                }
                else if(line.StartsWith("def"))
                {
                    this.CheckIdentifier(scope, line);
                    AddMethod(line, scope.Value);
                }
                else
                {
                    Lines.Add(new Line(line,this));
                }
                
            }
        }
        #endregion

        #region CodeObject

        public override Variable GetVariable(string var)
        {
            if(HasVariable(var))
            {
                return Variables.Single(x => x.Name == var);
            }
            return Parent.GetVariable(var);
        }
        public override void SetVariable(string var, string val)
        {
            if (HasVariable(var))
            {
                var v = Variables.Single(x => x.Name == var);
                var type = Helpers.GetType(val);
                if (v.Type != type)
                {
                    ThrowError(string.Format("Cannot assign {0} to a {1}}", type.ToString(), v.Type.ToString()));
                }
                else
                {
                    v.Value = val;
                }
            }
            Parent.SetVariable(var, val);
        }
        public override bool HasVariable(string var) 
        {
            if (Variables.Any(x => x.Name == var))
            {
                return Variables.Any(x => x.Name == var);
            }
            else if (Parent != null)
            {
                return Parent.HasVariable(var);
            }
            return false;
        }

        public override Data MakeMethodCall(MethodCall call)
        {
            if(Methods.Any(x=>x.Name == call.MethodName))
            {
                foreach(var item in Methods.Where(x => x.Name == call.MethodName))
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
