using DavcCompiler.help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DavcCompiler.help.Enums;

namespace DavcCompiler.Types
{
    public class Line : CodeObject
    {
        private static string[] _illeagleSymboles = { };
        public static string[] _delims = { "+", "-", "=", "*", "/" };
        public Line(string s, CodeObject parent)
        {
            Code = s;
            LineTokens = new List<LineToken>();
            parent = Parent;
            ParseCode(s);
            //Do parse work
        }
        public bool isEval { get; set; }
        public bool hasReturn { get; set; }
        public string Code { get; set; }
        public TypeEnum? ReturnType { get; set; }
        public List<LineToken> LineTokens { get; set; }

        public Data Eval()
        {
            foreach (var item in LineTokens)
            {
                item.Eval();
            }
            if (LineTokens.Count() == 1)
            {
                var token = LineTokens.Single();
                switch (token.Type)
                {
                    case TokenTypeEnum.Break:
                        break;
                    case TokenTypeEnum.Continue:
                        break;
                    default:
                        return token.value;
                }
            }
            else if (LineTokens.Count() == 1)
            {
                if (LineTokens[0].Type == TokenTypeEnum.Return)
                {
                    var value = LineTokens[1].value == null ? LineTokens[1].token : LineTokens[1].value.Value;
                    return new Data(value, Helpers.GetType(value));
                }
            }
            else if (LineTokens.Count() > 2)
            {
                var value = string.Empty;
                if (LineTokens[1].token == "=")
                {
                    if (LineTokens.Any(x => x.Type == TokenTypeEnum.String) && LineTokens.Where(x => x.Type == TokenTypeEnum.Operator).All(x => x.token == "+"))
                    {
                        for (var i = 2; i < LineTokens.Where(x => x.Type == TokenTypeEnum.Operator).Count(); i++)
                        {
                            var val = LineTokens[1].value == null ? LineTokens[1].token : LineTokens[1].value.Value;
                            value += val;
                        }
                        SetVariable(LineTokens[0].token, value);
                    }
                    else
                    {
                        ThrowError("Illeagle Operations at hand");
                    }
                }
                else
                {
                    if (LineTokens.Any(x => x.Type == TokenTypeEnum.String) && LineTokens.Where(x => x.Type == TokenTypeEnum.Operator).All(x => x.token == "+"))
                    {
                        for (var i = 2; i < LineTokens.Where(x => x.Type == TokenTypeEnum.Operator).Count(); i++)
                        {
                            var val = LineTokens[1].value == null ? LineTokens[1].token : LineTokens[1].value.Value;
                            value += val;
                        }
                        return new Data(value, TypeEnum.String);
                    }
                }
            }
            return null;
        }

        #region ParseMethods

        private string PrepareLine(string s)
        {
            var buffer = string.Empty;
            var inSingle = false;
            var inDouble = false;
            foreach (var item in s)
            {
                if (!inSingle && !inDouble)
                {

                    if (_delims.Contains(item.ToString()))
                    {
                        buffer += " ";
                        buffer += item;
                        buffer += " ";
                    }
                    else
                    {
                        buffer += item;
                    }
                }
                else
                {
                    if (item == '"')
                    {
                        inDouble = !inDouble;
                    }
                    else if (item == '\'')
                    {
                        inSingle = !inSingle;
                    }
                    else
                    {
                        buffer += item;
                    }
                }
            }
            return buffer;
        }
        private void ParseCode(string s)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                var tokens = PrepareLine(s).Trim().Split(' ');
                foreach (var token in tokens)
                {
                    LineTokens.Add(new LineToken(token, this));
                }
            }
        }
        #endregion
        #region CodeObject
        public override Variable GetVariable(string var)
        {
            return Parent.GetVariable(var);
        }

        public override void SetVariable(string var, string val)
        {
            Parent.SetVariable(var, val);
        }

        public override bool HasVariable(string var)
        {
            return Parent.HasVariable(var);
        }

        public override Data MakeMethodCall(MethodCall call)
        {
            return Parent.MakeMethodCall(call);
        }
        #endregion
    }
}
