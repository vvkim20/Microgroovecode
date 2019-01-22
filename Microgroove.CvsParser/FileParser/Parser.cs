using System;
using System.Linq;
using System.IO;
using Microgroove.CvsParser.FileParser.Validations;
using Microgroove.CvsParser.FileParser.ValueObject;
using Microgroove.CvsParser.Extensions;
using System.Collections.Generic;

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

        readonly int TOTAL_BUYER = 1;
        readonly int TOTAL_TIMINGS = 1;

        public Parser(IValidator headerv, IValidator footerv)
        {
            headerValidator = headerv;
            footerValidator = footerv;
        }

        public JsonOutputValue ReadCvsFile(string cvsFilePath)
        {
            JsonOutputValue result = new JsonOutputValue();

            if (File.Exists(cvsFilePath))
            {
                // Depend on the size of file, if huge file came in like 1GB or higher, it should use ReadLine instead of ReadAllLines
                var arrLines = File.ReadAllLines(cvsFilePath);

                // Check how many line of string parsed from the file
                if (arrLines.Count() <= 2)
                    new ApplicationException($"Please check the file, only less than or equal to 2 lines found from the file {cvsFilePath} ");


                // Parse header 
                var headerElements = GetHeader(arrLines[0]);
                result.Date = headerElements.date;
                result.Type = headerElements.type;


                // Parse Footer
                var footerElements = GetFooter(arrLines[arrLines.Length - 1]);
                result.Ender = new Ender()
                {
                    Created = footerElements.created,
                    Paid = footerElements.paid,
                    Process = footerElements.process
                };

                // Validate body
                // For body, we need to validate and parse at the the same time since it needs to loop twice               

                result.Orders = new List<Order>();

                // Loop for body
                for (var idx = 1; idx < arrLines.Length - 2; idx++)
                {

                    // Validate "O" first
                    if (!arrLines[idx].GetRecordType().In('O'))
                    {
                        new ApplicationException($"Expecting \"O\" but {arrLines[idx].GetRecordType()}");
                    }

                    // Get First element "O"
                    var orderDetail = GetOrder(arrLines[idx]);
                    var order = new Order();
                    order.Date = orderDetail.date;
                    order.Code = orderDetail.code;
                    order.Number = orderDetail.number;

                    var subIdx = idx;
                    var buyersCount = 0;
                    var timingsCount = 0;

                    order.Items = new List<Item>();
                    // Get All Record types until "O" or "E"
                    while (!arrLines[++subIdx].GetRecordType().In('O', 'E'))
                    {
                        switch (arrLines[subIdx].GetRecordType())
                        {
                            case 'B':
                                // O needs a single B
                                if (++buyersCount > TOTAL_BUYER)
                                    throw new ApplicationException($"Found more than max number of B : Total Buy : {TOTAL_BUYER}");
                                var buyer = GetBuyer(arrLines[subIdx]);
                                order.Buyer = new Buyer
                                {
                                    Name = buyer.name,
                                    Street = buyer.street,
                                    Zip = buyer.zip
                                };
                                break;
                            case 'L':
                                // O can contain Ls
                                var item = GetItem(arrLines[subIdx]);
                                order.Items.Add(new Item()
                                {
                                    Qty = item.qty,
                                    Sku = item.sku
                                });
                                break;
                            case 'T':
                                // O needs a single T
                                if (++timingsCount > TOTAL_TIMINGS)
                                    throw new ApplicationException($"Found more than max number of T : Total Timings: {TOTAL_BUYER}");
                                var timing = GetTiming(arrLines[subIdx]);
                                order.Timings = new Timings()
                                {
                                    Gap = timing.gap,
                                    Offset = timing.offset,
                                    Pause = timing.pause,
                                    Start = timing.start,
                                    Stop = timing.stop
                                };
                                break;

                            default:
                                throw new ApplicationException($"{arrLines[subIdx].GetRecordType()} is not valild record type for body");
                        }
                    }
                    idx = subIdx - 1;
                    if (buyersCount == 0 || timingsCount == 0)
                        throw new ApplicationException("Missing Timing or Buyer record for a order");
                    result.Orders.Add(order);
                }

            }
            else
            {
                throw new ApplicationException($"The file is not found by {cvsFilePath}");
            }
            return result;
        }

        #region Parser Section
        /// <summary>
        /// Todo: Should be injected method
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private (DateTime date, string type) GetHeader(string header)
        {
            // Validate header
            var headerValidation = headerValidator.DoValidate(header);
            if (!string.IsNullOrEmpty(headerValidation))
            {
                throw new ApplicationException(headerValidation);
            }

            var headerElements = header.Split(',');
            var date = headerElements[1].TrimQuotes().ParseDateTime();
            var type = headerElements[2].TrimQuotes();
            return (date, type);
        }

        /// <summary>
        /// Todo: Should be injected method
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        private (long process, long paid, long created) GetFooter(string footer)
        {
            // Valiate footer
            var footerValidation = footerValidator.DoValidate(footer);
            if (!string.IsNullOrEmpty(footerValidation))
            {
                throw new ApplicationException(footerValidation);
            }

            var footerElements = footer.Split(',');
            var process = footerElements[1].TrimQuotes().ParseLong();
            var paid = footerElements[2].TrimQuotes().ParseLong();
            var created = footerElements[3].TrimQuotes().ParseLong();
            return (process, paid, created);
        }
       

        private (DateTime date, string code, string number) GetOrder(string line)
        {
            // Validation need
            var orderElements = line.Split(',');
            var date = orderElements[1].TrimQuotes().ParseDateTime();
            var code = orderElements[2].TrimQuotes();
            var number = orderElements[3].TrimQuotes();
            return (date, code, number);
        }

        private (string name, string street, string zip) GetBuyer(string line)
        {
            // Validation need
            var orderElements = line.Split(',');
            var name = orderElements[1].TrimQuotes();
            var street = orderElements[2].TrimQuotes();
            var zip = orderElements[3].TrimQuotes();
            return (name, street, zip);
        }

        private (string sku, long qty) GetItem(string line)
        {
            // Validation need
            var itemElements = line.Split(',');
            var sku = itemElements[1].TrimQuotes();
            var qty = itemElements[2].TrimQuotes().ParseLong();
            return (sku, qty);
        }

        private (long start, long stop, long gap, long offset, long pause) GetTiming(string line)
        {
            // Validation need
            var timingElements = line.Split(',');
            var start = timingElements[1].TrimQuotes().ParseLong();
            var stop = timingElements[2].TrimQuotes().ParseLong();
            var gap = timingElements[3].TrimQuotes().ParseLong();
            var offset = timingElements[4].TrimQuotes().ParseLong();
            var pause = timingElements[5].TrimQuotes().ParseLong();
            return (start, stop, gap, offset, pause);
        }
        #endregion Parser Section
    }
}