using DavcCompiler.help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DavcCompiler.help.Enums;

namespace DavcCompiler.Types
{
    public class LineToken: CodeObject
    {
        public LineToken(string s, CodeObject parent)
        {
            Parent = parent;
            token = s;
            ParseToken(s);
            //do parse work
        }
        public string token { get; set; }
        public TokenTypeEnum Type { get; set; }
        public bool Done { get; set; }
        public Line MethodParams { get; set; }
        public Data value { get; set; }

        public void Eval()
        {
            switch(Type)
            {
                case TokenTypeEnum.Call:
                    var data = MethodParams.Eval();
                    var call = new MethodCall(token.Substring(0, token.IndexOf('(')));
                    value = MakeMethodCall(call);
                    Done = true;
                    break;
                case TokenTypeEnum.Number:
                    value = new Data(token, TypeEnum.Number);
                    break;
                case TokenTypeEnum.String:
                    value = new Data(token, TypeEnum.String);
                    break;
                case TokenTypeEnum.Variable:
                    var var = GetVariable(token);
                    value = new Data(var.Value, var.Type);
                    break;
                default:
                    value = null;
                    break;
            }
        }

        #region ParseMethods
        private void ParseToken(string s)
        {
            if (s.ContainsIgnoreLits('(') != -1)
            {
                Type = TokenTypeEnum.Call;
                var Params = s.GetTextBetween('(', ')');
                MethodParams = new Line(Params, this);

            }
            else if(s.isNumber())
            {
                Type = TokenTypeEnum.Number;
            }
            else if (s.isString())
            {
                Type = TokenTypeEnum.String;
            }
            else
            {
                switch(s)
                {
                    case "return":
                        Type = TokenTypeEnum.Return;
                        break;
                    case "null":
                        Type = TokenTypeEnum.Null;
                        break;
                    case "break":
                        Type = TokenTypeEnum.Break;
                        break;
                    case "continue":
                        Type = TokenTypeEnum.Continue;
                        break;
                    case "new":
                        Type = TokenTypeEnum.New;
                        break;
                    case "+":
                        Type = TokenTypeEnum.Operator;
                        break;
                    case "-":
                        Type = TokenTypeEnum.Operator;
                        break;
                    case "=":
                        Type = TokenTypeEnum.Operator;
                        break;
                    case "*":
                        Type = TokenTypeEnum.Operator;
                        break;
                    case "/":
                        Type = TokenTypeEnum.Operator;
                        break;
                    default:
                        Type = TokenTypeEnum.Variable;
                        break;

                }
            }
        }

        #endregion
        #region CodeObject
        public override Variable GetVariable(string var)
        {
            return Parent.GetVariable(var);
        }

        public override bool HasVariable(string var)
        {
            return Parent.HasVariable(var);
        }

        public override void SetVariable(string var, string val)
        {
            Parent.SetVariable(var, val);
        }

        public override Data MakeMethodCall(MethodCall call)
        {
            return Parent.MakeMethodCall(call);
        }
        #endregion
    }
}
