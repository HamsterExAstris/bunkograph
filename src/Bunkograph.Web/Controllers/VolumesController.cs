using Bunkograph.DAL;
using Bunkograph.Web.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bunkograph.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VolumesController : ControllerBase
    {
        private readonly BunkographContext _context;

        public VolumesController(BunkographContext context)
        {
            _context = context;
        }

        // GET: api/<VolumesController>
        [HttpGet("{language}")]
        public async IAsyncEnumerable<Volume> Get(string language)
        {
            await foreach (Models.BookEdition? volume in _context.BookEditions
                .Where(be => be.Language == language)
                .Include(be => be.Book)
                .ThenInclude(b => b.SeriesBooks)
                .AsAsyncEnumerable())
            {
                yield return new Volume(volume);
            }
        }

        // GET api/<VolumesController>/5
        // [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Models.BookEdition? bookEdition = await _context.BookEditions.FindAsync(id);
            return bookEdition is null
                ? NotFound()
                : Ok(new Volume(bookEdition));
        }

        // POST api/<VolumesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SeriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VolumesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
