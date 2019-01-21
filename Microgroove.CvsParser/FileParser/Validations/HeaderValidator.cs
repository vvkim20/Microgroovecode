using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.CvsParser.FileParser.Validations
{
    public class HeaderValidator : Validator
    {
        /// <summary>
        /// This should be inject with validation class 
        /// Todo: Build error message with string builder should be good so that validating entire file to output all errors
        /// </summary>
        /// <param name="headerLine"></param>
        /// <returns></returns>
        public override string DoValidate(string line)
        {
            var headerElements = line.Split(',');

            // Check number of element. Should be 3
            if (headerElements.Count() != 3)
                return "HE01 - Header elements should be 3";

            // Check all double quotes first element and the last elements
            if (!ValidateDoubleQuotes(headerElements))
            {
                return "HE02 - \" is required before and after of string : " + line;
            }

            // First Element Should be F
            if (headerElements[0] != "\"F\"")
                return "HE04 - Header code should be F but " + headerElements[0];

            // Second element should be Date format mm/dd/yyyy
            if (ValidateDateTimeFormat(headerElements[1]))
            {
                return "HE05 - Wrong format of datetime : " + headerElements[1];
            }

            // Thrid should be string
            if (string.IsNullOrEmpty(headerElements[2]))
            {
                return "HE06 - Empty string";
            }

            return string.Empty;
        }
    }
}
