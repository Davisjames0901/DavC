using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DavCRevolution.CodeObjects.Standard;
using DavCRevolution.Interfaces;
using DavCRevolution.Interfaces.ICodeObjects;
using DavCRevolution.Interfaces.ICodeObjects.ILine;

namespace DavCRevolution.lib
{
    public class Parser : IParser
    {
        private static List<string> SyntaxErrors { get; set; }

        public string FormatString(string s)
        {
            s.RemoveCharacters('\r', '\n', '\t');
            return s;
        }

        public ISourceFile ParseSourceFile(string file)
        {
            return CreateSourceFile(PrepareFile(file));
        }

        private List<string> PrepareFile(string file)
        {
            var program = new List<string>();
            var line = new StringBuilder();
            var buffer = new StringBuilder();
            var startFound = false;
            var inSingle = false;
            var inDouble = false;
            var depth = 0;
            foreach (var letter in file)
            {
                if (startFound)
                {
                    if (!inDouble && !inSingle)
                    {
                        switch (letter)
                        {
                            case '"':
                                inDouble = !inDouble;
                                buffer.Append(letter);
                                break;
                            case '\'':
                                inSingle = !inSingle;
                                buffer.Append(letter);
                                break;
                            case '}':
                                depth--;
                                break;
                            case '{':
                                depth++;
                                break;
                        }
                        if (!inDouble && !inSingle)
                        {
                            line.Append(letter);
                            if (depth == 0 && letter == '}')
                            {
                                program.Add(line.ToString());
                                line.Clear();
                            }
                        }
                    }
                    else
                    {
                        switch (letter)
                        {
                            case '"':
                                inDouble = !inDouble;
                                break;
                            case '\'':
                                inSingle = !inSingle;
                                break;
                            default:
                                break;
                        }
                        buffer.Append(letter);
                    }
                }
                else
                {
                    line.Append(letter);
                    if (letter == ';')
                    {
                        program.Add(line.ToString());
                        line.Clear();
                    }
                    else if (letter == ':')
                    {
                        program.Add(line.ToString());
                        line.Clear();
                        startFound = true;
                    }

                }
            }
            return Beautifier(program);
        }
        public List<string> Beautifier(List<string> program)
        {
            var start2Found = false;
            var i = 0;
            var beautiful = new List<string>();
            foreach (var item in program)
            {
                i++;
                if (!item.StartsWith("import") && !start2Found)
                {
                    if ((item.Trim().StartsWith("public") || item.Trim().StartsWith("private")) && item.Trim().EndsWith(":"))
                    {
                        start2Found = true;
                        beautiful.Add(item.Trim());
                    }
                    else
                    {
                        SyntaxErrors.Add($"Illegal statement found! \n Line:{i}   {item}");
                    }
                }
                else if (!start2Found)
                {

                    if (item.StartsWith("import"))
                    {
                        beautiful.Add(item.Trim());
                    }
                    else
                    {
                        SyntaxErrors.Add($"Illegal statement found! \n Line:{i}   {item}");
                    }
                }
                else if (start2Found)
                {
                    if ((item.Contains("def") && !item.StartsWith("def")) || (item.Contains("class") && !item.StartsWith("class")))
                    {
                        var clsInx = item.IndexOf("class");
                        var defInx = item.IndexOf("def");
                        if (defInx < clsInx && defInx != -1)
                        {
                            var token = item.Substring(0, item.IndexOf("def"));
                            if (token.Contains("=") && token.EndsWith(";"))
                            {
                                beautiful.Add(token.Trim());
                                beautiful.Add(item.Substring(item.IndexOf("def"), item.Length - token.Length));
                            }
                            else
                            {
                                SyntaxErrors.Add($"Illegal statement found! \n Line:{i}   {item}");
                            }
                        }
                        else
                        {
                            var token = item.Substring(0, item.IndexOf("class"));
                            if (token.Contains("=") && token.EndsWith(";"))
                            {
                                beautiful.Add(token.Trim());
                                beautiful.Add(item.Substring(item.IndexOf("class"), item.Length - token.Length));
                            }
                            else
                            {
                                SyntaxErrors.Add($"Illegal statement found! \n Line:{i}   {item}");
                            }
                        }
                    }

                    else if (item.StartsWith("def") || item.StartsWith("class"))
                    {
                        beautiful.Add(item.Trim());
                    }
                }
            }
            return beautiful;
        }

        private ISourceFile CreateSourceFile(List<string> file)
        {
            var sourceFile = new SourceFile();



            return sourceFile;
        }

        private IMethod CreateMethod(IEnumerable<string> text)
        {
            var method = new Method();
            foreach (var line in text)
            {
                method.Lines.ToList().Add(CreateLine(line));
            }
            return method;
        }

        private ILine CreateLine(string line)
        {
            return new ComputeLine();
        }
    }
}
