using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab3.Models
{
    public class Book
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Articul { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<Reader> Readers { get; set; }

        public Book()
        {
            Readers = new List<Reader>();
        }
    }
}
