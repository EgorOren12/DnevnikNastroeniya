using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DnevnikNastroeniya.Models
{
    internal class Record
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public TimeOnly TimeOfCreation { get; set; }
        public int Mood_Level { get; set; }
        public string Comment { get; set; }
        public DateTime DateOfCreationNote { get; set; }
        public int IdClient { get; set; }
        public int IdScale { get; set; }
        [ForeignKey(nameof(IdClient))]
        public Client? Client { get; set; }

        [ForeignKey(nameof(IdScale))]
        public Mood_Scale? Scale {  get; set; }

        public List<Tag> Tags { get; set; } = new();

        public List<Factor_Type> Factor_Types { get; set; } = new();
    }
}
