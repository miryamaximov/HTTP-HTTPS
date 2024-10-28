using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
using System.Text.Json;

namespace HTTP_HTTPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagmentStudentsController : ControllerBase
    {
        private List<Student> _students = new List<Student>();
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\Courses\\Projects\\HTTP_HTTPS\\HTTP_HTTPS\\students.json");

        public ManagmentStudentsController()
        {
            //  LoadStudentsFromJson();
            _students.Add(new Student { Id = 1, Name = "יוסי כהן", Age = 15 });
        }

        // JSON טעינת רשימת התלמידים מקובץ 
        private void LoadStudentsFromJson()
        {
            string json = System.IO.File.ReadAllText(filePath);
            _students = JsonSerializer.Deserialize<List<Student>>(json);
        }

        // שמירת התלמידים לקובץ לאחר ביצוע שינויים ברשימה
        private void SaveStudents()
        {
             System.IO.File.WriteAllText(filePath, JsonSerializer.Serialize(_students));
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (_students.Count < 1)
                return NotFound();  // 404 code 
            return Ok(_students); // 200 code 
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Student student = _students.FirstOrDefault(k => k.Id == id);
            if (student is null)
                return NotFound();  // 404 code 
            return Ok(student);  // 200 code 
        }

        [HttpPost]
        public IActionResult Post(Student student)
        {
            if (student is null)
                return BadRequest("The student object is null..."); // 400 code 
            if (string.IsNullOrEmpty(student.Name))
                return BadRequest("Name field is required!"); // 400 code
            _students.Add(student);
            SaveStudents();
            return CreatedAtAction(nameof(Get), new { id = student.Id }, student);  // 201 code 
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Student student = _students.FirstOrDefault(k => k.Id == id);
            if (student is null)
                return NotFound();  // 404  code
            _students.Remove(student);
            SaveStudents();
            return NoContent();  // 204 code
        }
    }
}
