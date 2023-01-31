using lab3.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace lab3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadersController : ControllerBase
    {
        ApplicationContext db;
        public ReadersController(ApplicationContext context)
        {
            db = context;
            if (!db.Readers.Any())
            {
                Reader rd = new Reader
                {
                    FLName = "Зарипов Владислав Игоревич",
                    BirthDate = new DateTime(2001, 01, 16),
                };
                db.Readers.Add(rd);

                db.Books.Add(new Book
                {
                    Name = "Life",
                    Author = "Robert",
                    Articul = "123gsdf",
                    Year = 1953,
                    Count = 5,
                    Readers = new List<Reader> { rd }
                });

                db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reader>>> Get()
        {
            return await db.Readers.Include(x => x.Books).ToListAsync();
        }

        [HttpGet("count")]
        public ActionResult<int> GetCount()
        {
            return db.Readers.Count(); ;
        }

        // GET api/readers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reader>> Get(int id)
        {
            Reader reader = await db.Readers.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
            if (reader == null)
                return NotFound("Not found reader");
            return new ObjectResult(reader);
        }

        // POST api/readers
        [HttpPost]
        public async Task<ActionResult<Reader>> Post(Reader reader)
        {
            if (reader == null)
            {
                return BadRequest("Empty argument");
            }

            db.Readers.Add(reader);
            await db.SaveChangesAsync();
            return Ok(reader);
        }

        // PUT api/readers
        [HttpPut]
        public async Task<ActionResult<Reader>> Put(Reader reader)
        {
            if (reader == null)
            {
                return BadRequest("Empty argument");
            }
            if (!db.Readers.Any(x => x.Id == reader.Id))
            {
                return NotFound("Not found reader");
            }

            db.Update(reader);
            await db.SaveChangesAsync();
            return Ok(reader);
        }

        // DELETE api/readers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reader>> Delete(int id)
        {
            Reader reader = db.Readers.Include(x => x.Books).FirstOrDefault(x => x.Id == id);
            if (reader == null)
            {
                return NotFound("Not found reader");
            }
            db.Readers.Remove(reader);
            await db.SaveChangesAsync();
            return Ok(reader);
        }

        // PUT api/readers/{id} - add book
        [HttpPut("{id}/addbook")]
        public async Task<ActionResult<Reader>> Put(int id, [FromBody] string name)
        {
            if (name == null)
            {
                return BadRequest("Empty argument");
            }
            Book book = db.Books.FirstOrDefault(x => x.Name == name);
            if (book == null)
            {
                return NotFound("Not found book");
            }
            Reader reader = db.Readers.Include(x => x.Books).FirstOrDefault(x => x.Id == id);
            if (reader == null)
            {
                return NotFound("Not found reader");
            }
            if (!reader.Books.Any(x => x == book))
                reader.Books.Add(book);
            await db.SaveChangesAsync();
            return Ok(reader);
        }

        // Delete api/readers/{id} - delete book
        [HttpDelete("{id}/deletebook")]
        public async Task<ActionResult<Reader>> Delete(int id, [FromBody] string name)
        {
            if (name == null)
            {
                return BadRequest("Empty argument");
            }
            Book book = db.Books.FirstOrDefault(x => x.Name == name);
            if (book == null)
            {
                return NotFound("Not found book");
            }
            Reader reader = db.Readers.Include(x => x.Books).FirstOrDefault(x => x.Id == id);
            if (reader == null)
            {
                return NotFound("Not found reader");
            }
            if (reader.Books.Any(x => x == book))
                reader.Books.Remove(book);
            else
                return NotFound("Not found reader's book");
            db.Readers.Update(reader);
            await db.SaveChangesAsync();
            return Ok(reader);
        }

        // POSt api/readers/search
        [HttpPost("search")]
        public ActionResult<Reader> Post([FromBody] string name)
        {
            IQueryable<Reader> readers = db.Readers.Include(x => x.Books);
            if (!String.IsNullOrEmpty(name))
            {
                readers = readers.Where(p => p.FLName.Contains(name));
            } else
            {
                return BadRequest("Empty argument");
            }
            if (readers.IsNullOrEmpty())
                return NotFound("Not found readers");
            return new ObjectResult(readers);
        }
    }
}
