using FluentAssertions;
using Microgroove.CvsParser.FileParser;
using System;
using Xunit;
using Moq;
using Microgroove.CvsParser.FileParser.Validations;
using System.IO;

namespace Microgroove.CvsParser.FileParser
{
    public class ParserTest
    {


        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void ShouldParseCsv()
        {
            // Arrange
            var mock = new Mock<IValidator>();
            mock.Setup(f => f.DoValidate(It.IsAny<string>())).Returns("");

            var parser = new Parser(mock.Object, mock.Object);

            // Act
            var result = parser.ReadCvsFile($"{Directory.GetCurrentDirectory()}\\SampleFiles\\TestInput.csv");

            // Asserts
            
        }


    }
}
