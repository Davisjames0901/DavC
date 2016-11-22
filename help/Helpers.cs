using DavcCompiler.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using static DavcCompiler.help.Enums;

namespace DavcCompiler.help
{
    public static class Helpers
    {
        
        public static char[] _delims = { '*', '+', '-', '=', '/' };
        public static string RemoveCharacters(this string s, params char[] c)
        {
            bool inLiteral = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '"' || s[i] == '\'')
                {
                    inLiteral = !inLiteral;
                }
                if (c.Contains(s[i]) && !inLiteral)
                {
                    s = s.Remove(i, 1);
                    i--;
                }
            }
            return s;
        }
        public static string[] SplitIgnoreLits(this string s, char[] delims)
        {
            var inLit = false;
            var tokens = new List<string>();
            string token = string.Empty;
            foreach (var letter in s)
            {
                if (letter == '\"' || letter == '\'')
                {
                    inLit = !inLit;
                    token += letter;
                }
                else if (delims.Contains(letter) && !inLit)
                {
                    tokens.Add(token);
                    tokens.Add(letter.ToString());
                    token = string.Empty;
                }
                else
                {
                    token += letter;
                }
            }
            if (!string.IsNullOrWhiteSpace(token))
            {
                tokens.Add(token);
            }
            return tokens.ToArray();
        }
        public static int ContainsIgnoreLits(this string s, char c)
        {
            var inLit = false;
            var i = 0;
            foreach (var letter in s)
            {
                if (letter == '"' || letter == '\'')
                {
                    inLit = !inLit;
                }
                if (letter == c)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
        public static bool isString(this string s)
        {
            return s.StartsWith("\"") && s.EndsWith("\"");
        }
        public static string TrimBy(this string s, int trimAmount = 1)
        {
            s = s.Remove(0, trimAmount);
            s = s.Remove(s.Length - 1, trimAmount);
            return s;
        }
        public static bool isNumber(this string s)
        {
            var isNumber = true;
            try
            {
                double num;
                num = double.Parse(s);
            }
            catch
            {
                isNumber = false;
            }
            return isNumber;
        }
        public static TypeEnum GetType(string s)
        {
            return TypeEnum.String;
        }
        public static string GetTextBetween(this string s, char start, char end)
        {
            var startIndex = s.IndexOf(start);
            var first = s.Substring(startIndex+1, (s.Length-1)-startIndex);
            var endIndex = first.LastIndexOf(end);
            var second = first.Substring(0, endIndex);
            return second;
        }
        public static List<string> Beautify(this string s)
        {
            var temp = new List<string>();
            var buffer = string.Empty;
            var inLiteral = false;
            var inLiteralDouble = false;
            foreach (var item in s)
            {
                if (!inLiteral && !inLiteralDouble)
                {
                    switch (item)
                    {
                        case '\'':
                            inLiteral = !inLiteral;
                            buffer += item;
                            break;
                        case '"':
                            inLiteralDouble = !inLiteralDouble;
                            buffer += item;
                            break;
                        case ':':
                            buffer += item;
                            temp.Add(buffer);
                            buffer = string.Empty;
                            break;
                        case '}':
                            buffer += item;
                            temp.Add(buffer);
                            buffer = string.Empty;
                            break;
                        default:
                            buffer += item;
                            break;
                    }
                }
                else
                {
                    switch (item)
                    {
                        case '\'':
                            inLiteral = !inLiteral;
                            buffer += item;
                            break;
                        case '"':
                            inLiteralDouble = !inLiteralDouble;
                            buffer += item;
                            break;
                        default:
                            buffer += item;
                            break;
                    }
                }
            }
            var BeauCode = new List<string>();
            foreach (var item in temp)
            {
                if(item.StartsWith("def")||item.EndsWith(":"))
                {
                    BeauCode.Add(item);
                }
                else
                {
                    var variable = item.Substring(0, item.IndexOf("def"));
                    BeauCode.Add(variable);
                    var method = item.Substring(item.IndexOf("def"), item.Length - variable.Length);
                }
            }
            return BeauCode;
        }
        public static List<string> BeautifyMethod(this string s)
        {
            var code = new List<string>();
            var buffer = string.Empty;
            var inLiteral = false;
            var inLiteralDouble = false;
            foreach (var item in s)
            {
                if (!inLiteral && !inLiteralDouble)
                {
                    switch (item)
                    {
                        case '\'':
                            inLiteral = !inLiteral;
                            buffer += item;
                            break;
                        case '"':
                            inLiteralDouble = !inLiteralDouble;
                            buffer += item;
                            break;
                        case '}':
                            buffer += item;
                            code.Add(buffer);
                            buffer = string.Empty;
                            break;
                        case ';':
                            buffer += item;
                            code.Add(buffer);
                            buffer = string.Empty;
                            break;
                        default:
                            buffer += item;
                            break;
                    }
                }
                else
                {
                    switch (item)
                    {
                        case '\'':
                            inLiteral = !inLiteral;
                            buffer += item;
                            break;
                        case '"':
                            inLiteralDouble = !inLiteralDouble;
                            buffer += item;
                            break;
                        default:
                            buffer += item;
                            break;
                    }
                }
            }
            return code;
        }
        public static ScopeTypeEnum ParseIdentifier(string s)
        {
            s = s.Remove(s.Length - 1, 1);
            switch (s)
            {
                case "public":
                    return ScopeTypeEnum.Public;
                case "private":
                    return ScopeTypeEnum.Private;
                default:
                    throw new Exception(string.Format("{0} is not a valid access modifier", s));
            }
        }
        public static TypeEnum EvaluateTypeToken(string s)
        {
            switch(s)
            {
                case "num":
                    return TypeEnum.Number;
                case "string":
                    return TypeEnum.String;
                default:
                    throw new Exception(string.Format("{0} is not a validType"));
            }
        }
        public static void CheckIdentifier(this CodeObject obj, ScopeTypeEnum? scope, string method)
        {
            if (!scope.HasValue)
            {
                obj.ThrowError(string.Format("Class entity doesnt exist inside Access modifier./nLine:/n{0}", method));
            }
        }

    }
}
