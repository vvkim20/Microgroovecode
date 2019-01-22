using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.CvsParser.Extensions
{
    public static class ParserExtesions
    {
        public static char GetRecordType(this string line)
        {
            return line.Substring(0, 3).TrimQuotes()[0];
        }

        
    }
}
