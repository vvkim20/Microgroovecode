using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.CvsParser.FileParser.ValueObject
{
    /// <summary>
    /// Value Object for CVS file
    /// </summary>
    public class JsonOutputValue
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public List<Order> Orders { get; set; }
        public Ender Ender { get; set; }
    }

    public class Ender
    {
        public int Process { get; set; }
        public int Paid { get; set; }
        public int Created { get; set; }
    }

    public class Order
    {
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string Number { get; set; }
        public Buyer Buyer { get; set; }
        public List<Item> Items { get; set; }
        public Timings Timings { get; set; }
    }

    public class Timings
    {
        public long Start { get; set; }
        public long Stop { get; set; }
        public long Gap { get; set; }
        public long Offset { get; set; }
        public long Pause { get; set; }
    }

    public class Buyer
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
    }
    public class Item
    {
        public string Sku { get; set; }
        public long Qty { get; set; }
    }
}
