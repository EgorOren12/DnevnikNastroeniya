using System;
using System.Collections.Generic;
using System.Text;

namespace DnevnikNastroeniya.Models
{
    internal class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Record> Notes { get; set; } = new();
    }
}
