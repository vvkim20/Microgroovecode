using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.CvsParser.FileParser.Validations
{
    public interface IValidator
    {
        string DoValidate(string line);
    }

    public abstract class Validator : IValidator
    {
        public abstract string DoValidate(string line);

        /// <summary>
        ///  Validate double quotes
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected bool ValidateDoubleQuotes(string[] element)
        {
            foreach (var s in element)
            {
                if (s.Length > 2)
                {
                    if (s[0] == '"' && s[s.Length - 1] == '"')
                        continue;
                    return false;
                }

                if (s.Length != 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Validate DateTime format
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected bool ValidateDateTimeFormat(string line)
        {
            return !DateTime.TryParseExact(line.TrimStart('"').TrimEnd('"'), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date);
        }
    }
}
