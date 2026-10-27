using System;
using System.Collections.Generic;
using System.Text;

namespace DnevnikNastroeniya.Models
{
    internal class Client
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime DateOfCreate { get; set; }
    }
}
