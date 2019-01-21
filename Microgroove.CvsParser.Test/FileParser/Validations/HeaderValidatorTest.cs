using FluentAssertions;
using Microgroove.CvsParser.FileParser.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microgroove.CvsParser.Test.FileParser.Validations
{
    public class HeaderValidatorTest
    {
        /// <summary>
        /// Validate all double quotes since file could be corrupted
        /// </summary>
        /// <param name="headerString"></param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData("\"F\",\"08/04/2018\",\" By Batch #\"", "")]
        [InlineData("\"F\",\"08/04/2018\",\" By Batch #", "HE02 - \" is required before and after of string : \"F\",\"08/04/2018\",\" By Batch #")]
        [InlineData("\"F\",08/04/2018\",\" By Batch #\"", "HE02 - \" is required before and after of string : \"F\",08/04/2018\",\" By Batch #\"")]
        [InlineData("\"F\",\"08/04/2018\",", "HE06 - Empty string")]
        [InlineData("\"F\",\"14/04/2018\",\" By Batch #\"", "HE05 - Wrong format of datetime : \"14/04/2018\"")]
        [InlineData("\"X\",,", "HE04 - Header code should be F but \"X\"")]
        public void Should_True_HeaderValidation(string headerString, string expected)
        {
            // Arrange
            var validator = new HeaderValidator();

            // Act
            var result = validator.DoValidate(headerString);

            // Asserts
            result.Should().Be(expected);
        }
    }
}
