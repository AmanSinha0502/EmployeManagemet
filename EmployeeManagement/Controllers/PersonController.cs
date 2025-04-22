using Dapper;
using DapperMvcDemo.Models;
using DapperMvcDemo.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EmployeeManagement.Models.Entities;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly string connectionString;

        public PersonController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Get all persons
        [HttpGet]
        public IActionResult GetAllPersons()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var persons = connection.Query<Person>("SELECT * FROM person").ToList();
                return Ok(persons);
            }
        }

        // Get person by ID
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetPersonById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var person = connection.QuerySingleOrDefault<Person>("SELECT * FROM person WHERE id = @Id", new { Id = id });

                if (person == null)
                {
                    return NotFound();
                }

                return Ok(person);
            }
        }

        // Add new person
        [HttpPost]
        public IActionResult AddPerson(AddPersonDto addPersonDto)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var personEntity = new Person()
                {
                    Name = addPersonDto.Name,
                    Email = addPersonDto.Email,
                    Address = addPersonDto.Address,
                    DeptId = addPersonDto.DeptId
                };

                var query = "INSERT INTO person (name, email, address, deptid) VALUES (@Name, @Email, @Address, @DeptId)";
                connection.Execute(query, personEntity);

                return Ok(personEntity);
            }
        }

        // Update person by ID
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdatePerson(int id, UpdatePersonDto updatePersonDto)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var person = connection.QuerySingleOrDefault<Person>("SELECT * FROM person WHERE id = @Id", new { Id = id });

                if (person == null)
                {
                    return NotFound();
                }

                var updateQuery = "UPDATE person SET name = @Name, email = @Email, address = @Address, deptid = @DeptId WHERE id = @Id";
                person.Name = updatePersonDto.Name;
                person.Email = updatePersonDto.Email;
                person.Address = updatePersonDto.Address;
                person.DeptId = updatePersonDto.DeptId;

                connection.Execute(updateQuery, person);
                return Ok(person);
            }
        }

        // Delete person by ID
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeletePerson(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var person = connection.QuerySingleOrDefault<Person>("SELECT * FROM person WHERE id = @Id", new { Id = id });

                if (person == null)
                {
                    return NotFound();
                }

                connection.Execute("DELETE FROM person WHERE id = @Id", new { Id = id });
                return Ok();
            }
        }
    }
}
