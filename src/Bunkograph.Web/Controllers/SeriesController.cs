using Bunkograph.DAL;
using Bunkograph.Models;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bunkograph.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly BunkographContext _context;

        public SeriesController(BunkographContext context)
        {
            _context = context;
        }

        // GET: api/<SeriesController>
        [HttpGet]
        public IAsyncEnumerable<Series> Get()
        {
            return _context.Series.AsAsyncEnumerable();
        }

        // GET api/<SeriesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Series? series = await _context.Series.FindAsync(id);
            return series is null
                ? NotFound()
                : Ok(series);
        }

        // POST api/<SeriesController>
        [HttpPost]
        public async Task Post([FromBody] Series value)
        {
            _context.Series.Add(value);
            await _context.SaveChangesAsync();
        }

        // PUT api/<SeriesController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Series value)
        {
            Series? series = await _context.Series.FindAsync(id);
            if (series is not null)
            {
                _context.Entry(series).CurrentValues.SetValues(value);
            }
            else
            {
                _context.Series.Add(value);
            }
            await _context.SaveChangesAsync();
        }

        // DELETE api/<SeriesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            Series? series = await _context.Series.FindAsync(id);
            if (series is not null)
            {
                _context.Series.Remove(series);
                await _context.SaveChangesAsync();
            }
        }
    }
}
