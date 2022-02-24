using System.ComponentModel.DataAnnotations.Schema;

namespace API_T2.Models
{
    [Table("Persons")]
    public class Person
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public int Age { get; set; }

        public override bool Equals(object obj)
        {            
            return this.name == ((Person)obj).name;
        }
    }
}
