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
    public class FooterValidatorTest
    {
        [Theory]
        [InlineData("\"E\",\"1\",\"2\",\"9\"", "")]
        [InlineData("\"E\",\",,", "FE02 - \" is required before and after of string : \"E\",\",,")]
        [InlineData("\"X\",,,", "FE03 - Footer code should be E but \"X\"")]
        public void Should_True_FooterValidation(string footerString, string expected)
        {
            // Arrange
            var validator = new FooterValidator();

            // Act
            var result = validator.DoValidate(footerString);

            // Asserts
            result.Should().Be(expected);
        }
    }
}
