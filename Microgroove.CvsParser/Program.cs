using Microgroove.CvsParser.FileParser;
using Microgroove.CvsParser.FileParser.Validations;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Microgroove.CvsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            /// DI setup for console app for webjob, skip this for now
            /// https://andrewlock.net/using-dependency-injection-in-a-net-core-console-application/
            var parser = new Parser(new HeaderValidator(), new FooterValidator());
            var inputFileName = args[0];
            var outputFileName = $"{inputFileName }_Output.json";

            var result = parser.ReadCvsFile(inputFileName);
            var json = JsonConvert.SerializeObject(result);

            if (!File.Exists(outputFileName))
            {
                File.WriteAllText(outputFileName, json);
            }
        }
    }
}
