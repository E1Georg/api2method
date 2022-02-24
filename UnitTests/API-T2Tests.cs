using API_T2.Controllers;
using API_T2.Data;
using API_T2.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

// Перед проведение unit-тестов необходимо закомментировать в конструкторе подключения к БД(data/MyDBContext.cs) строки
// Database.EnsureDeleted(); и Database.EnsureCreated(); <- иначе данные в БД удаляться.
// Пересоздание db было добавлено, согласно общему заданию на работу
namespace API_T2.Tests
{
    public class API_T2Tests
    {
        [Fact]
        public void fullListWithPerson()
        {
            // arrange           

            using (MyDBContext context = new MyDBContext())
            {
                IEnumerable<object> expected_1 = context.Persons.Select(p => new { p.id, p.name, p.sex }).Where(p => p.sex == "male").ToList();
                IEnumerable<object> expected_2 = context.Persons.Select(p => new { p.id, p.name, p.sex }).Where(p => p.sex == "female").ToList();
                IEnumerable<object> expected_3 = context.Persons.Select(p => new { p.id, p.name, p.sex }).Where(p => p.sex == null).ToList(); 
                IEnumerable<object> expected_4 = context.Persons.Select(p => new { p.id, p.name, p.sex }).Where(p => p.sex == "").ToList();
                IEnumerable<object> expected_5 = context.Persons.Where(p => p.Age <= 17).Where(p => p.Age >= 14).ToList();
                IEnumerable<object> expected_6 = context.Persons.Where(p => p.Age <= 14).Where(p => p.Age >= 17).ToList();

                // act
                PersonsController controller = new PersonsController(new MyDBContext());          
               
                IEnumerable<object> actual_1 = controller.full("male");
                IEnumerable<object> actual_2 = controller.full("female");
                IEnumerable<object> actual_3 = controller.full(null);
                IEnumerable<object> actual_4 = controller.full("");
                IEnumerable<object> actual_5 = controller.full(null, minAge: 14, maxAge: 17);
                IEnumerable<object> actual_6 = controller.full("male", minAge: 17, maxAge: 14);
                
                // assert
                Assert.Equal(expected_1.ToString(), actual_1.ToString());
                Assert.Equal(expected_2.ToString(), actual_2.ToString());
                Assert.Equal(expected_3.ToString(), actual_3.ToString());
                Assert.Equal(expected_4.ToString(), actual_4.ToString());
                Assert.Equal(expected_5.ToString(), actual_5.ToString());
                Assert.Equal(expected_6.ToString(), actual_6.ToString());
            }
        }

        [Fact]
        public void singlePerson()
        {
            // arrange
            Person expected_1 = new Person { id = 1, name = "Stan Smith", sex = "male", Age = 30 };
            Person expected_2 = new Person { id = 5, name = "German Titov", sex = "male", Age = 42 };
            Person expected_3 = new Person();
            Person expected_4 = new Person();
            Person expected_5 = new Person();

            // act
            PersonsController controller = new PersonsController(new MyDBContext());

            Person actual_1 = controller.single(1);
            Person actual_2 = controller.single(5);
            Person actual_3 = controller.single(0);
            Person actual_4 = controller.single(-4);
            Person actual_5 = controller.single(124);

            // assert
            Assert.Equal(expected_1, actual_1);
            Assert.Equal(expected_2, actual_2);
            Assert.Equal(expected_3, actual_3);
            Assert.Equal(expected_4, actual_4);
            Assert.Equal(expected_5, actual_5);

        }
    }
}
