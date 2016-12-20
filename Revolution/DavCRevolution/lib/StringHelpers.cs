using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavCRevolution.lib
{
    public static class StringHelpers
    {
        public static string RemoveCharacters(this string s, params char[] c)
        {
            var inLiteral = false;
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == '"' || s[i] == '\'')
                {
                    inLiteral = !inLiteral;
                }
                if (!c.Contains(s[i]) || inLiteral)
                {
                    continue;
                }
                s = s.Remove(i, 1);
                i--;
            }
            return s;
        }
    }
}
