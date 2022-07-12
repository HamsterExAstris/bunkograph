using Bunkograph.DAL;
using Bunkograph.Web.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bunkograph.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolumesController : ControllerBase
    {
        private readonly BunkographContext _context;

        public VolumesController(BunkographContext context)
        {
            _context = context;
        }

        // GET: api/<VolumesController>/series/5
        [HttpGet("series/{seriesId}")]
        public async IAsyncEnumerable<Volume> GetBySeries(int seriesId)
        {
            await foreach (Models.SeriesBook? seriesBook in _context.SeriesBooks
                .Where(sb => sb.Series.SeriesId == seriesId)
                .Include(sb => sb.Book)
                .ThenInclude(b => b.Editions)
                .ThenInclude(be => be.SeriesLicense)
                .AsAsyncEnumerable())
            {
                foreach (Models.BookEdition? bookEdition in seriesBook.Book.Editions)
                {
                    yield return new Volume(seriesBook, bookEdition);
                }
            }
        }

        // GET api/<VolumesController>/5
        public async Task<IActionResult> Get(int id)
        {
            Models.BookEdition? bookEdition = await _context.BookEditions.FindAsync(id);
            return bookEdition is null
                ? NotFound()
                : throw new NotImplementedException(); // Ok(new Volume(bookEdition));
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
