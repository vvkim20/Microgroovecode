using Microgroove.CvsParser.FileParser.ValueObject;
using System;
using System.Linq;
using System.Text;
using System.IO;
using Microgroove.CvsParser.FileParser.Validations;
using Microgroove.CvsParser.Extensions;

namespace Microgroove.CvsParser.FileParser
{
    public interface IParser
    {
        JsonOutputValue ReadCvsFile(string cvsFilePath);
    }

    public class Parser : IParser
    {
        IValidator headerValidator;
        IValidator footerValidator;

        public Parser(IValidator headerv, IValidator footerv)
        {
            headerValidator = headerv;
            footerValidator = footerv;
        }

        public JsonOutputValue ReadCvsFile(string cvsFilePath)
        {
            if (File.Exists(cvsFilePath))
            {
                // Depend on the size of file, if huge file came in like 1GB or higher, it should use ReadLine instead of ReadAllLines
                var arrLInes = File.ReadAllLines(cvsFilePath);

                // Check how many line of string parsed from the file
                if (arrLInes.Count() <= 2)
                    new ApplicationException($"Please check the file, only less than or equal to 2 lines found from the file {cvsFilePath} ");

                JsonOutputValue result = new JsonOutputValue();

                // Validate header
                var headerValidation = headerValidator.DoValidate(arrLInes[0]);

                // Parse header to object
                var headerElements = GetHeader(arrLInes[0]);
                result.Date = headerElements.date;
                result.Type = headerElements.type;

                // Valiate footer
                var footer = footerValidator.DoValidate(arrLInes[arrLInes.Length - 1]);

                // Parse Footer
                var footerElements = GetFooter(arrLInes[arrLInes.Length - 1]);
                result.Ender = new Ender()
                                {
                                    Created = footerElements.created,
                                    Paid = footerElements.paid,
                                    Process = footerElements.process
                                };

                // Validate body
                // For body, we need to validate and parse at the the same time since it needs to loop twice               

                // Loop for body
                for (var idx = 1; idx < arrLInes.Length - 2; idx++)
                {
                    // First "O"
                    // Get "O" until until other "O" or "E"

                    // O needs a single B
                    // O needs a single T

                    // O can contain Ls
                    
                }
                
            }


            return new JsonOutputValue();
        }


        /// <summary>
        /// Todo: Should be injected method
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private (DateTime date, string type) GetHeader(string header)
        {
            var headerElements = header.Split(',');
            var date = headerElements[1].TrimFirstLast('"').ParseDateTime();
            var type = headerElements[2].TrimFirstLast('"');
            return (date, type);
        }

        /// <summary>
        /// Todo: Should be injected method
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        private (long process, long paid, long created) GetFooter(string footer)
        {
            var footerElements = footer.Split(',');
            var process = long.Parse(footerElements[1].TrimQuotes());
            var paid = long.Parse(footerElements[2].TrimQuotes());
            var created = long.Parse(footerElements[3].TrimQuotes());
            return (process, paid, created);
        }

        public string BodyValidation(string bodyLine)
        {
            return string.Empty;
        }
    }
}