using lab3.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;

namespace lab3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        ApplicationContext db;
        public BooksController(ApplicationContext context)
        {
            db = context;
            if (!db.Books.Any())
            {
                db.Books.Add(new Book
                {
                    Name = "Ocean",
                    Author = "Albert",
                    Articul = "48313g",
                    Year = 2005,
                    Count = 10
                });
                db.Books.Add(new Book
                {
                    Name = "Nsk",
                    Author = "Ivan",
                    Articul = "185faoifs",
                    Year = 2010,
                    Count = 3
                });

                db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            return await db.Books.ToListAsync();
        }

        // GET api/books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            Book book = await db.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
                return NotFound("Not found book");
            return new ObjectResult(book);
        }

        // POST api/books
        [HttpPost]
        public async Task<ActionResult<Book>> Post(Book book)
        {
            if (book == null)
            {
                return BadRequest("Empty argument");
            }

            db.Books.Add(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }

        // PUT api/readers
        [HttpPut]
        public async Task<ActionResult<Book>> Put(Book book)
        {
            if (book == null)
            {
                return BadRequest("Empty argument");
            }
            if (!db.Books.Any(x => x.Id == book.Id))
            {
                return NotFound("Not found book");
            }

            db.Books.Update(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> Delete(int id)
        {
            Book book = db.Books.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound("Not found book");
            }
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }

      
        // POST api/books/search
        [HttpPost("search")]
        public ActionResult<Book> Post([FromBody] string name)
        {
            IQueryable<Book> books = db.Books;
            if (!String.IsNullOrEmpty(name))
            {
                books = books.Where(p => p.Name.Contains(name));
            }
            else
            {
                return BadRequest("Empty argument");
            }
            if (books.IsNullOrEmpty())
                return NotFound("Not found books");
            return new ObjectResult(books);
        }
    }
}

