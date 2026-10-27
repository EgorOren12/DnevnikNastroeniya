using System;
using System.Collections.Generic;
using System.Text;

namespace DnevnikNastroeniya.Models
{
    internal class Mood_Scale
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Min_Value { get; set; }
        public int Max_Value { get; set; }
    }
}
