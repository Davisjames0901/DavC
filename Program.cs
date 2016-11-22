using DavcCompiler.help;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DavcCompiler.Types;
using System.Text;

namespace DavcCompiler
{
    class Program
    {
        private static List<string> SyntaxErrors;
        private static Dictionary<Guid, string> Strings;

        public Program(SourceFile program)
        {
            program.Start();
        }
       
        static void Main(string[] args)
        {
            SyntaxErrors = new List<string>();
            Strings = new Dictionary<Guid, string>();
            Console.WriteLine("Input File Path.");
            var filePath = Console.ReadLine();
            var source = File.ReadAllText("C:\\temp\\test.dav").RemoveCharacters('\r', '\n', '\t');
            var formattedSource = prepareFile(source);
            var test = new SourceFile("main", formattedSource);
            test.Start();
            if (!SyntaxErrors.Any())
            {
                try
                {
                    new Program(new SourceFile("main", formattedSource));
                }
                catch(Exception e)
                {
                    Console.WriteLine("Syntax Error: Please fix and try again.");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                foreach(var item in SyntaxErrors)
                {
                    Console.WriteLine(item);
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
		
        private static List<string> prepareFile(string s)
        {
            var program = new List<string>();
            var line = new StringBuilder();
            var buffer = new StringBuilder();
            var startFound = false;
            var inSingle = false;
            var inDouble = false;
            var depth = 0;
            foreach (var letter in s)
            {
                if (startFound)
                {
                    if (!inDouble && !inSingle)
                    {
                        if(buffer.Length > 0)
                        {
                            var guid = Guid.NewGuid();
                            Strings.Add(guid, buffer.ToString());
                            buffer.Clear();
                            line.Append(guid.ToString());
                        }
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
                        if(!inDouble && !inSingle)
                        {
                            line.Append(letter);
                            if(depth == 0 && letter == '}')
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

        public static List<string> Beautifier(List<string> program)
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
                        SyntaxErrors.Add(string.Format("Illegal statement found! \n Line:{0}   {1}", i, item));
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
                        SyntaxErrors.Add(string.Format("Illegal statement found! \n Line:{0}   {1}", i, item));
                    }
                }
                else if (start2Found)
                {
                    if ((item.Contains("def") && !item.StartsWith("def"))||(item.Contains("class") && !item.StartsWith("class")))
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
                                SyntaxErrors.Add(string.Format("Illegal statement found! \n Line:{0}   {1}", i, item));
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
                                SyntaxErrors.Add(string.Format("Illegal statement found! \n Line:{0}   {1}", i, item));
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
    }
}