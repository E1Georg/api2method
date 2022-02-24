using System.ComponentModel.DataAnnotations.Schema;

namespace API_T2.Models
{
    public class ConvertPerson
    {
        // Используется для получения данных с сайта(согласно заданию).
        public string id { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public int Age { get; set; }
    }
}
