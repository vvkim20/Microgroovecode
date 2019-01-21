using Microgroove.CvsParser.FileParser.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microgroove.CvsParser.FileParser.Validations;

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
                
                // Validate header
                var headerValidation = headerValidator.DoValidate(arrLInes[0]);

                // Valiate footer
                var footer = footerValidator.DoValidate(arrLInes[arrLInes.Length - 1]);
                
                // Validate body
                // For body, we need to validate and parse at the the same time since it needs to loop twice

                // 
                
            }


            return new JsonOutputValue();
        }


        public string BodyValidation(string bodyLine)
        {
            return string.Empty;
        }
    }
}