using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using API_T2.Models;
using API_T2.Data;


namespace API_T2.Controllers
{   
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly MyDBContext _context;
        public PersonsController(MyDBContext context)
        {
            _context = context;                      
        }

        [Route("/{controller}/{action}/{genderFilter?}")]
        [Route("/{controller}/{action}/{minAge}-{maxAge}")]
        [HttpGet]
        public IEnumerable<object> full(string genderFilter, int minAge = 0, int maxAge = 0)
        {
            // Если указан возраст(от и до), то вернется выборка жителей по возрасту
            // Иначе, если указан гендер(male/female), то вернётся выборка по определённому полу
            // Иначе вернётся весь список жителей
            if ((maxAge != 0) && (minAge != 0)) return _context.Persons.Where(p => p.Age <= maxAge).Where(p => p.Age >= minAge).ToList();
            else if (genderFilter == null || genderFilter == "") return _context.Persons.Select(p => new { p.id, p.name, p.sex }).ToList();          
            else return _context.Persons.Select(p => new { p.id, p.name, p.sex }).Where(p => p.sex == genderFilter).ToList();              
        }

        [Route("/{controller}/{action}/{id?}")]
        [HttpGet]
        public Person single(int id)
        {
            // По переданному id вернутся сведения об определённом жителе            
            var temp = _context.Persons.FirstOrDefault(p => p.id == id);
            if (temp == null) return new Person();
            else return temp;            
        }
    }
}
