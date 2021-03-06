﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microgroove.CvsParser.Extensions;
namespace Microgroove.CvsParser.FileParser.Validations
{
    public class FooterValidator : Validator
    {
        public override string DoValidate(string line)
        {
            var footerElements = line.Split(',');

            // Check number of element. Should be 4
            if (footerElements.Count() != 4)
                return "FE01 - Footer elements should be 4";

            // Check all double quotes first element and the last elements
            if (!ValidateDoubleQuotes(footerElements))
            {
                return "FE02 - \" is required before and after of string : " + line;
            }

            // Record Type Should be E
            if (!footerElements[0].GetRecordType().In('E'))
                return "FE03 - Footer code should be E but " + footerElements[0];

            return string.Empty;
        }
    }
}
