using DavcCompiler.help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DavcCompiler.help.Enums;

namespace DavcCompiler.Types
{
    public class Method : CodeObject
    {
        private static string[] _illeagleSymboles = { };
        public Method(string s, CodeObject parent, ScopeTypeEnum access)
        {
            Variables = new List<Variable>();
            Code = new List<Line>();
            Parameters = new List<Parameters>();
            Parent = parent;
            AccessLevel = access;
            Name = ParseName(s);
            ParseCode(s);
            //do parse work here
        }
        //set method signature to name
        public List<Variable> Variables { get; set; }
        public List<Parameters> Parameters { get; set; }
        public List<Line> Code { get; set; }
        public string ReturnValue { get; set; }
        public TypeEnum? ReturnType { get; set; }
        public ScopeTypeEnum AccessLevel { get; set; }

        public void AddVariable(string var, string val, ScopeTypeEnum access)
        {
            var variable = new Variable(var,val, this, access);
            if (HasVariable(variable.Name) || Parent.HasVariable(variable.Name))
            {
                SetVariable(var, val);
            }
            Variables.Add(variable);
        }
        public Data ExecuteMethod(MethodCall call)
        {
            var i = 0;
            foreach(var var in Parameters)
            {
                if(!HasVariable(var.Name))
                {
                    if (call.Parameters[i].Type == var.Type)
                    {
                        Variables.Add(new Variable(var.Name, call.Parameters[i].Value, this, ScopeTypeEnum.Local));
                    }
                    else
                    {
                        ThrowError(string.Format("{0} is not assignable to {1}", call.Parameters[i].Type, var.Type));
                    }
                }
                else
                {
                    ThrowError(string.Format("var {0} cannot be a parameter because that variable already exists.", var.Name));
                }
                i++;
            }
            var value = string.Empty;
            foreach(var line in Code)
            {
                var data = line.Eval();
                if(data != null)
                {
                    return data;
                }
            }
            return new Data(value, ReturnType ?? TypeEnum.Void);
        }
        #region ParseMethods
        private string ParseName(string s)
        {
            return s.Substring(3, s.IndexOf('(')-3).Trim();
        }
        private void ParseParameters(string s)
        {
            var parameters = s.Substring(s.IndexOf('(')+1, s.IndexOf(')') - s.IndexOf('(')-1);
            if (parameters.Length > 3)
            {
                foreach (var item in parameters.Split(','))
                {
                    Parameters.Add(new Parameters(item));
                }
            }
        }
        private void ParseCode(string s)
        {
            ParseParameters(s);
            var code = s.GetTextBetween('{', '}');
            var lines = code.BeautifyMethod();
            foreach(var line in lines)
            {
                Code.Add(new Line(line, this));
            }
            if(Code.Any(x=>x.hasReturn))
            {
                ReturnType = Code.Single(x => x.hasReturn).ReturnType.Value;
            }
        }
        #endregion

        #region CodeObject
        public override Variable GetVariable(string var)
        {
            if (HasVariable(var))
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
            return Parent.MakeMethodCall(call);
        }
        #endregion
    }
}
