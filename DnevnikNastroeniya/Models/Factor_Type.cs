using System;
using System.Collections.Generic;
using System.Text;

namespace DnevnikNastroeniya.Models
{
    public class Factor_Type 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Record> Notes { get; set; } = new();
    }
}
