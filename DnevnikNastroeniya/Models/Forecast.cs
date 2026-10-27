using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DnevnikNastroeniya.Models
{
    internal class Forecast
    {
        public int Id { get; set; }
        public DateTime Target_Date { get; set; }
        public string Mood_Forecast {  get; set; }
        public string Confidence { get; set; }
        public string State { get; set; }
        public DateTime CreationDate { get; set; }

        public int IdNote { get; set; }

        [ForeignKey(nameof(IdNote))]
        public Note? Note { get; set; }
    }
}
